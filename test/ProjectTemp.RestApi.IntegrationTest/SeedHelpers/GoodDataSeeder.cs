using System;
using System.Threading.Tasks;
using Project.Domain.Aggregates.Goods;
using Project.Domain.Aggregates.Goods.ValueObjects;
using Project.Infrastructure;

namespace Project.RestApi.IntegrationTest.SeedHelpers
{
    public static class GoodDataSeeder
    {
        public static async Task Seed(WriteDbContext dbContext)
        {
            var firstGood = Good.Create(
                GoodId.Create(FirstGoodId),
                GoodName.Create(FirstGoodName),
                GoodPrice.Create(FirstGoodPrice),
                GoodDiscount.Create(FirstGoodDiscount),
                GoodIsFragile.Create(FirstGoodIsFragile),
                UserDataSeeder.AdminUserId);
            dbContext.Goods.Add(firstGood);

            var secondGood = Good.Create(
                GoodId.Create(SecondGoodId),
                GoodName.Create(SecondGoodName),
                GoodPrice.Create(SecondGoodPrice),
                GoodDiscount.Create(SecondGoodDiscount),
                GoodIsFragile.Create(SecondGoodIsFragile),
                UserDataSeeder.AdminUserId);
            dbContext.Goods.Add(secondGood);

            await dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        #region First Good Fields

        public const string FirstGoodName = "string";

        public const decimal FirstGoodPrice = 100000;

        public const decimal FirstGoodDiscount = 10;

        private static readonly Guid FirstGoodId = Guid.NewGuid();

        private const bool FirstGoodIsFragile = false;

        #endregion

        #region Second Good Fields

        public const string SecondGoodName = "second_string";

        public const decimal SecondGoodPrice = 10000;

        public const decimal SecondGoodDiscount = 20;

        private static readonly Guid SecondGoodId = Guid.NewGuid();

        private const bool SecondGoodIsFragile = true;

        #endregion
    }
}