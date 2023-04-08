using System.Threading;
using System.Threading.Tasks;
using Project.Domain.Aggregates.Goods;

namespace Project.Application.Aggregates.Goods
{
    public interface IGoodWriteRepository
    {
        void Add(Good good);

        Task<Good> GetByName(string goodName, CancellationToken cancellationToken = default);

        void Remove(Good good);
    }
}