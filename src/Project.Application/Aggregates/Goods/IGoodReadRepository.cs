using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Project.Application.Aggregates.Goods.Queries;

namespace Project.Application.Aggregates.Goods
{
    public interface IGoodReadRepository
    {
        IQueryable<GoodQueryResult> GetAll();

        Task<GoodQueryResult> GetByName(string goodName, CancellationToken cancellationToken = default);
    }
}