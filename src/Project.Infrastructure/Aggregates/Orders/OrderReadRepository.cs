using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
                    SELECT
		                    O.""Id"",
		                    O.""Status"",
		                    O.""PostType"",
		                    O.""Description"",
		                    COALESCE ( UC.""Username"", O.""CreatorId"" :: TEXT ) AS ""Creator"",
		                    COALESCE ( UU.""Username"", O.""UpdaterId"" :: TEXT ) AS ""Updater"",
		                    O.""Created"",O.""Updated"",
		                    B.""TotalPrice""
                    FROM
		                    ""Orders"" AS O
		                    LEFT JOIN ""Users"" AS UC ON O.""CreatorId"" = UC.""Id""
		                    LEFT JOIN ""Users"" AS UU ON O.""UpdaterId"" = UU.""Id""
		                    JOIN (
		                    SELECT A.""Id"",
					                    SUM (A.""TotalPrice"") AS ""TotalPrice""
	                      FROM
		                    (
				                    SELECT
						                    O.""Id"",
						                    G.""Price"",
						                    G.""Discount"",
						                    ((G.""Price"" - (G.""Price"" * ( G.""Discount"" / 100 ))) * OI.""Count"") :: INT AS ""TotalPrice""
				                    FROM
						                    ""Orders"" AS O
						                    INNER JOIN ""OrderItems"" AS OI ON O.""Id"" = OI.""OrderId""
						                    INNER JOIN ""Goods"" AS G ON OI.""GoodId"" = G.""Id""
						                    ) AS A
		                    GROUP BY
		                    ""Id"") AS B ON O.""Id"" = B.""Id""");
        }

        public Task<OrderQueryResult> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            return dbContext.OrderQueryResults.FromSqlRaw(@"
                    SELECT
		                    O.""Id"",
		                    O.""Status"",
                            O.""PostType"",
		                    O.""Description"",
		                    COALESCE ( UC.""Username"", O.""CreatorId"" :: TEXT ) AS ""Creator"",
		                    COALESCE ( UU.""Username"", O.""UpdaterId"" :: TEXT ) AS ""Updater"",
		                    O.""Created"",O.""Updated"",
		                    B.""TotalPrice""
                    FROM
		                    ""Orders"" AS O
		                    LEFT JOIN ""Users"" AS UC ON O.""CreatorId"" = UC.""Id""
		                    LEFT JOIN ""Users"" AS UU ON O.""UpdaterId"" = UU.""Id""
		                    JOIN (
		                    SELECT A.""Id"",
					                    SUM (A.""TotalPrice"") AS ""TotalPrice""
	                      FROM
		                    (
				                    SELECT
						                    O.""Id"",
						                    G.""Price"",
						                    G.""Discount"",
						                    ((G.""Price"" - (G.""Price"" * ( G.""Discount"" / 100 ))) * OI.""Count"") :: INT AS ""TotalPrice""
				                    FROM
						                    ""Orders"" AS O
						                    INNER JOIN ""OrderItems"" AS OI ON O.""Id"" = OI.""OrderId""
						                    INNER JOIN ""Goods"" AS G ON OI.""GoodId"" = G.""Id""
						                    ) AS A
		                    GROUP BY
		                    ""Id"") AS B ON O.""Id"" = B.""Id""
                    WHERE o.""Id"" = {0}
                ", id).FirstOrDefaultAsync(cancellationToken);
        }

        public Task<OrderItemQueryResult[]> GetOrderItems(Guid orderId, CancellationToken cancellationToken = default)
        {
            return dbContext.OrderItemQueryResults.FromSqlRaw(@"
                    SELECT G.* 
                    FROM ""OrderItems"" AS OI 
                    INNER JOIN ""Goods"" AS G ON OI.""GoodId"" = G.""Id""
                    WHERE OI.""OrderId"" = {0}
                    ", orderId)
                .ToArrayAsync(cancellationToken);
        }
    }
}