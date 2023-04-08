using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Project.Application.Aggregates.Orders.Queries;

namespace Project.Application.Aggregates.Orders
{
    public interface IOrderReadRepository
    {
        IQueryable<OrderQueryResult> GetAll();

        Task<OrderQueryResult> GetById(Guid id, CancellationToken cancellationToken = default);
    }
}