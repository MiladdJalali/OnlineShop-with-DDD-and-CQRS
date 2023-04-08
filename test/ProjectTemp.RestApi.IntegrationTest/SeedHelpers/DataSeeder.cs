using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Project.Infrastructure;

namespace Project.RestApi.IntegrationTest.SeedHelpers
{
    public static class DataSeeder
    {
        public static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetRequiredService<WriteDbContext>();

            await UserDataSeeder.Seed(dbContext).ConfigureAwait(false);
            await GoodDataSeeder.Seed(dbContext).ConfigureAwait(false);

            await dbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}