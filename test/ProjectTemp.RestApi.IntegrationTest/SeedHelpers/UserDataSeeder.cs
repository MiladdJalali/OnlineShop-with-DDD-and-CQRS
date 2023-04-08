using System;
using System.Threading.Tasks;
using Project.Application;
using Project.Domain.Aggregates.Users;
using Project.Domain.Aggregates.Users.ValueObjects;
using Project.Infrastructure;

namespace Project.RestApi.IntegrationTest.SeedHelpers
{
    public static class UserDataSeeder
    {
        public static async Task Seed(WriteDbContext dbContext)
        {
            SeedAdminUser(dbContext);

            await dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        private static void SeedAdminUser(WriteDbContext dbContext)
        {
            var user = User.Create(
                UserId.Create(AdminUserId),
                UserUsername.Create(AdminUserUsername),
                UserPassword.Create(AdminUserPassword),
                UserAddress.Create(AdminAddress),
                Guid.Empty);

            dbContext.Users.Add(user);
        }

        #region Admin User Fields

        public static readonly Guid AdminUserId = Guid.NewGuid();

        private const string AdminUserUsername = "string";

        private static readonly string AdminUserPassword = "string".GetHash();

        private const string AdminAddress = "string";

        #endregion
    }
}