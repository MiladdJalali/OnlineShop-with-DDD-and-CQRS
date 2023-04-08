using Autofac;
using MediatR.Extensions.Autofac.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Npgsql;
using Project.Application;
using Project.Application.Behaviors;
using Project.Application.Services;
using Project.Domain.Aggregates.Orders.Services;
using Project.Infrastructure;
using Project.Infrastructure.Services;
using Project.RestApi.Services;

namespace Project.RestApi
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddHttpContextAccessor();
            services.AddLogging(configuration);
            services.AddApiVersioning(options => options.ReportApiVersions = true);
            services.AddControllers();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo {Title = "REST API", Version = "v1"});
                options.AddSecurityDefinition("basic", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    In = ParameterLocation.Header,
                    Description = "Basic Authorization header using the Bearer scheme."
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "basic"
                            }
                        },
                        new string[] { }
                    }
                });
            });

            services.AddDbContextPool<WriteDbContext>(i => { i.UseNpgsql(connectionString); });
            services.AddDbContextPool<ReadDbContext>(i => { i.UseNpgsql(connectionString); });

            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            services.AddScoped<IUserService, UserService>();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            var applicationAssembly = typeof(ApplicationConstants).Assembly;
            var infrastructureAssembly = typeof(WriteDbContext).Assembly;
            builder.RegisterAssemblyTypes(infrastructureAssembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterType<SystemDateTime>().As<ISystemDateTime>().SingleInstance();

            builder.RegisterMediatR(applicationAssembly, typeof(LoggingBehavior<,>), typeof(TransactionBehavior<,>));
            builder.RegisterType<SystemEntityDetector>().As<ISystemEntityDetector>().InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<UserDescriptor>().As<IUserDescriptor>().InstancePerLifetimeScope();
            builder.RegisterType<PasswordHashProvider>().As<IPasswordHashProvider>().InstancePerLifetimeScope();
            builder.RegisterType<GoodsTotalPriceValidator>().As<IGoodsTotalPriceValidator>().InstancePerLifetimeScope();
            builder.RegisterType<OrderTimeValidator>().As<IOrderTimeValidator>().InstancePerLifetimeScope();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            WriteDbContext dbContext,
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IServiceScopeFactory serviceScopeFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "REST API v1"));
            }

            dbContext.Database.Migrate();

            var dbConnection = dbContext.Database.GetDbConnection();
            dbConnection.Open();
            ((NpgsqlConnection) dbConnection).ReloadTypes();
            dbConnection.Close();

            DatabaseSeeder.Seed(serviceScopeFactory, configuration).GetAwaiter().GetResult();

            app.UseRouting();
            app.UseGlobalException();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}