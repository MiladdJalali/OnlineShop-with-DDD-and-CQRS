using System.Threading;
using System.Threading.Tasks;

namespace Project.Application
{
    public interface IUnitOfWork
    {
        Task BeginTransaction(CancellationToken cancellationToken = default);

        Task CommitTransaction(CancellationToken cancellationToken = default);

        Task RollbackTransaction(CancellationToken cancellationToken = default);
    }
}