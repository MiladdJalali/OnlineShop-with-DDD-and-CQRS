using Project.Domain.Aggregates.Orders.ValueObjects;

namespace Project.Domain.Aggregates.Orders.Events
{
    public class OrderDeletedEvent : BaseDomainEvent
    {
        public OrderDeletedEvent(OrderId orderId)
            : base(orderId.Value)
        {
        }
    }
}