using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Project.Domain.Aggregates.Orders.Services;
using Project.Domain.Aggregates.Orders.ValueObjects;

namespace Project.Infrastructure.Services
{
    public class GoodsTotalPriceValidator : IGoodsTotalPriceValidator
    {
        private readonly WriteDbContext dbContext;

        public GoodsTotalPriceValidator(WriteDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> IsTotalPriceValid(OrderItem[] items)
        {
            var goodIds = items.Select(i => i.GoodId.Value);
            var goods = await dbContext.Goods
                .FromSqlRaw(@$"SELECT * FROM ""Goods"" WHERE ""Id"" IN('{string.Join("','", goodIds)}')")
                .AsNoTracking()
                .ToArrayAsync();

            decimal totalPrice = 0;
            foreach (var good in goods)
            {
                var count = items.Where(i => i.GoodId == good.Id).Sum(i => i.Count);
                totalPrice +=
                    (good.Price.Value - good.Price.Value * (good.Discount.Value / 100)) * count;
            }

            return totalPrice >= 50000;
        }
    }
}