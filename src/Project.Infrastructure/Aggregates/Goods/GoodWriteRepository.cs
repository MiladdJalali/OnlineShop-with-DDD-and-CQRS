using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Project.Application.Aggregates.Goods;
using Project.Domain.Aggregates.Goods;

namespace Project.Infrastructure.Aggregates.Goods
{
    public class GoodWriteRepository : IGoodWriteRepository
    {
        private readonly WriteDbContext dbContext;

        public GoodWriteRepository(WriteDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Add(Good good)
        {
            dbContext.Goods.Add(good);
        }

        public Task<Good> GetByName(string goodName, CancellationToken cancellationToken = default)
        {
            return dbContext.Goods.FromSqlRaw(@"
                    SELECT      G.*
                    FROM        ""Goods"" AS G
                    WHERE       G.""Name"" = {0}
                ", goodName)
                .SingleOrDefaultAsync(cancellationToken);
        }

        public void Remove(Good good)
        {
            good.Delete();
            dbContext.Goods.Remove(good);
        }
    }
}