using Project.Domain.Aggregates.Orders.Enums;
using Project.Domain.Aggregates.Orders.ValueObjects;

namespace Project.Domain.Aggregates.Orders.Events
{
    public class OrderCreatedEvent : BaseDomainEvent
    {
        public OrderCreatedEvent(OrderId orderId, OrderStatus orderStatus)
            : base(orderId.Value)
        {
            Status = orderStatus.ToString();
        }

        public string Status { get; }
    }
}