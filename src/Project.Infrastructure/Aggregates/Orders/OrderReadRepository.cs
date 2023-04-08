using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Project.Application.Aggregates.Goods.Queries;
using Project.Application.Aggregates.Orders;
using Project.Application.Aggregates.Orders.Queries;

namespace Project.Infrastructure.Aggregates.Orders
{
    public class OrderReadRepository : IOrderReadRepository
    {
        private readonly ReadDbContext dbContext;

        public OrderReadRepository(ReadDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IQueryable<OrderQueryResult> GetAll()
        {
            return dbContext.OrderQueryResults.FromSqlRaw(@"
                    SELECT      O.""Id"",
                                O.""Status"",
                                O.""Description"",
                                COALESCE(UC.""Username"", O.""CreatorId""::TEXT) AS ""Creator"",
                                COALESCE(UU.""Username"", O.""UpdaterId""::TEXT) AS ""Updater"",
                                O.""Created"",
                                O.""Updated""
                    FROM        ""Orders"" AS O
                    LEFT JOIN   ""Users""       AS UC   ON O.""CreatorId""      = UC.""Id""
                    LEFT JOIN   ""Users""       AS UU   ON O.""UpdaterId""      = UU.""Id""");
        }

        public async Task<OrderQueryResult> GetById(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var order = await GetOrder(id, cancellationToken).ConfigureAwait(false);
            order.Items = await GetGoodItems(order.Id, cancellationToken).ConfigureAwait(false);
            order.Items.ToList().ForEach(i => i.Good = GetItem(i.GoodId, cancellationToken).GetAwaiter().GetResult());

            order.TotalPrice = CalculateTotalPrice(order.Items.Select(i => i.Good).ToArray());

            return order;
        }

        private Task<GoodQueryResult> GetItem(
            Guid goodId,
            CancellationToken cancellationToken = default)
        {
            return dbContext.GoodQueryResults.FromSqlRaw(@"
                    SELECT      G.""Id"",
                                G.""Name"",
                                G.""Price"",
                                G.""Discount"",
                                G.""IsFragile"",
                                G.""Description"",
                                COALESCE(UC.""Username"", G.""CreatorId""::TEXT) AS ""Creator"",
                                COALESCE(UU.""Username"", G.""UpdaterId""::TEXT) AS ""Updater"",
                                G.""Created"",
                                G.""Updated""
                    FROM        ""Goods"" AS G
                    LEFT JOIN   ""Users""       AS UC   ON G.""CreatorId""      = UC.""Id""
                    LEFT JOIN   ""Users""       AS UU   ON G.""UpdaterId""      = UU.""Id""
                    WHERE       G.""Id"" = {0}
                    ", goodId).FirstOrDefaultAsync(cancellationToken);
        }

        private Task<OrderItemQueryResult[]> GetGoodItems(
            Guid orderId,
            CancellationToken cancellationToken = default)
        {
            return dbContext.OrderItemQueryResults.FromSqlRaw(@"
                    SELECT      OI.""OrderId"",
                                OI.""GoodId"",
                                OI.""OrderPostType"",
                                OI.""Count""                    
                    FROM        ""OrderItems"" AS OI                    
                    WHERE       OI.""OrderId"" = {0}
                ", orderId)
                .ToArrayAsync(cancellationToken);
        }

        private Task<OrderQueryResult> GetOrder(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return dbContext.OrderQueryResults.FromSqlRaw(@"
                    SELECT      O.""Id"",
                                O.""Status"",
                                O.""Description"",
                                COALESCE(UC.""Username"", O.""CreatorId""::TEXT) AS ""Creator"",
                                COALESCE(UU.""Username"", O.""UpdaterId""::TEXT) AS ""Updater"",
                                O.""Created"",
                                O.""Updated""
                    FROM        ""Orders"" AS O
                    LEFT JOIN   ""Users""       AS UC   ON O.""CreatorId""      = UC.""Id""
                    LEFT JOIN   ""Users""       AS UU   ON O.""UpdaterId""      = UU.""Id""
                    WHERE       O.""Id"" = {0}
                ", id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        private static decimal CalculateTotalPrice(GoodQueryResult[] goods)
        {
            var sumPrices = goods.Sum(i => i.Price);
            var sumDiscounts = goods.Sum(i => i.Price * i.Discount / 100);

            return sumPrices - sumDiscounts;
        }
    }
}