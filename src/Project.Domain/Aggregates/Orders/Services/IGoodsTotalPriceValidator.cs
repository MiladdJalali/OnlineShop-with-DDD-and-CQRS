using System.Threading.Tasks;
using Project.Domain.Aggregates.Orders.ValueObjects;

namespace Project.Domain.Aggregates.Orders.Services
{
    public interface IGoodsTotalPriceValidator
    {
        Task<bool> IsTotalPriceValid(OrderItem[] goods);
    }
}