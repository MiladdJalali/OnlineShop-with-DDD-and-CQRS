using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Project.Application.Aggregates.Goods;
using Project.Application.Aggregates.Goods.Queries;

namespace Project.Infrastructure.Aggregates.Goods
{
    public class GoodReadRepository : IGoodReadRepository
    {
        private readonly ReadDbContext dbContext;

        public GoodReadRepository(ReadDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IQueryable<GoodQueryResult> GetAll()
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
                    LEFT JOIN   ""Users""       AS UU   ON G.""UpdaterId""      = UU.""Id""");
        }

        public Task<GoodQueryResult> GetByName(
            string goodName,
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
                    WHERE       G.""Name"" = {0}
                ", goodName).FirstOrDefaultAsync(cancellationToken);
        }
    }
}