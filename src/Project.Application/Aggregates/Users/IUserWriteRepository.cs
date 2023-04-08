using System.Threading;
using System.Threading.Tasks;
using Project.Domain.Aggregates.Users;

namespace Project.Application.Aggregates.Users
{
    public interface IUserWriteRepository
    {
        void Add(User user);

        Task<User> GetByUsername(string username, CancellationToken cancellationToken = default);

        void Remove(User user);
    }
}