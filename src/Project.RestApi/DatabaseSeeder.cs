using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Project.Application;
using Project.Domain.Aggregates.Users;
using Project.Domain.Aggregates.Users.ValueObjects;
using Project.Infrastructure;

namespace Project.RestApi
{
    public static class DatabaseSeeder
    {
        public static async Task Seed(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
        {
            using var scope = serviceScopeFactory.CreateScope();

            var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();
            var dbContext = scope.ServiceProvider.GetService<WriteDbContext>();

            try
            {
                await unitOfWork.BeginTransaction().ConfigureAwait(false);
                SeedUsers(dbContext, configuration);
                await unitOfWork.CommitTransaction().ConfigureAwait(false);
            }
            catch
            {
                await unitOfWork.RollbackTransaction().ConfigureAwait(false);
            }
        }

        private static void SeedUsers(WriteDbContext dbContext, IConfiguration configuration)
        {
            if (dbContext.Users.Any() && dbContext.Users.Any(i => i.Id.Value == ApplicationConstants.AdminUserId))
                return;

            dbContext.Users.Add(
                User.Create(
                    UserId.Create(ApplicationConstants.AdminUserId),
                    UserUsername.Create(ApplicationConstants.AdminUsername),
                    UserPassword.Create(configuration["AdminPassword"]!.GetHash()),
                    UserAddress.Create(ApplicationConstants.AdminAddress),
                    ApplicationConstants.AdminUserId)
            );
        }
    }
}