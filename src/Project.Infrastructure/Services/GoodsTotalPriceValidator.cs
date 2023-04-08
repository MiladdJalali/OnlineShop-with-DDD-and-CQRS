using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Project.Domain.Aggregates.Orders.Services;

namespace Project.Infrastructure.Services
{
    public class GoodsTotalPriceValidator : IGoodsTotalPriceValidator
    {
        private readonly WriteDbContext dbContext;

        public GoodsTotalPriceValidator(WriteDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> IsTotalPriceValid(IEnumerable<Guid> goodIds)
        {
            var goods = await dbContext.Goods
                .FromSqlRaw(@$"SELECT * FROM ""Goods"" WHERE ""Id"" IN('{string.Join("','", goodIds)}')")
                .AsNoTracking()
                .ToArrayAsync();

            return goods.Sum(t => t.Price.Value) >= 50000;
        }
    }
}