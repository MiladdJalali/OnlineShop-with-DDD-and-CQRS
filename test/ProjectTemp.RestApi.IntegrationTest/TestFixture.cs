using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Project.Domain.Aggregates.Orders.Services;
using Project.Infrastructure;
using Project.RestApi.IntegrationTest.Fakes;
using Project.RestApi.IntegrationTest.SeedHelpers;
using Project.RestApi.Services;

namespace Project.RestApi.IntegrationTest
{
    public class TestFixture : IDisposable
    {
        private readonly TestServer testServer;

        public TestFixture()
        {
            var configurationRoot = new ConfigurationBuilder()
                .AddJsonStream(new MemoryStream(
                        Encoding.ASCII.GetBytes(
                            JsonSerializer.Serialize(new
                            {
                                ConnectionStrings = new
                                {
                                    DefaultConnection =
                                        "Server=LocalHost;" +
                                        $"Database=Project{DateTime.Now.Ticks};" +
                                        "User Id=postgres;" +
                                        "Password=;"
                                }
                            })
                        )
                    )
                ).Build();

            var builder = new WebHostBuilder()
                .ConfigureServices(services => services.AddAutofac())
                .UseEnvironment("Production")
                .UseConfiguration(configurationRoot)
                .UseStartup<Startup>()
                .ConfigureServices(services =>
                {
                    services.AddScoped<IUserService, UserService>();
                    services.AddScoped<IOrderTimeValidator, FakeOrderTimeValidator>();
                });

            testServer = new TestServer(builder);

            DataSeeder.EnsureSeedData(testServer.Services).GetAwaiter().GetResult();
            Client = testServer.CreateClient();
            Client.DefaultRequestHeaders.Add("Authorization", "Basic c3RyaW5nOnN0cmluZw==");
        }

        public HttpClient Client { get; }

        public void Dispose()
        {
            var dbContext = testServer.Services.GetRequiredService<WriteDbContext>();

            dbContext.Database.EnsureDeleted();
            Client.Dispose();
            testServer.Dispose();
        }
    }
}