using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Project.Application.Aggregates.Orders;
using Project.Domain.Aggregates.Orders;

namespace Project.Infrastructure.Aggregates.Orders
{
    public class OrderWriteRepository : IOrderWriteRepository
    {
        private readonly WriteDbContext dbContext;

        public OrderWriteRepository(WriteDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Add(Order order)
        {
            dbContext.Orders.Add(order);
        }

        public Task<Order> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            return dbContext.Orders.FromSqlRaw(@"
                    SELECT      O.*
                    FROM        ""Orders"" AS O
                    WHERE       O.""Id"" = {0}
                ", id)
                .SingleOrDefaultAsync(cancellationToken);
        }

        public void Remove(Order order)
        {
            order.Delete();
            dbContext.Orders.Remove(order);
        }
    }
}