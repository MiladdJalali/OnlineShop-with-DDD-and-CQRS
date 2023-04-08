using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Project.Application.Aggregates.Users.Queries;

namespace Project.Application.Aggregates.Users
{
    public interface IUserReadRepository
    {
        IQueryable<UserQueryResult> GetAll();

        Task<UserQueryResult> GetByUsername(string username, CancellationToken cancellationToken = default);
    }
}