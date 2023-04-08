using System;
using System.Threading;
using System.Threading.Tasks;
using Project.Domain.Aggregates.Orders;

namespace Project.Application.Aggregates.Orders
{
    public interface IOrderWriteRepository
    {
        void Add(Order order);

        Task<Order> GetById(Guid id, CancellationToken cancellationToken = default);

        void Remove(Order order);
    }
}