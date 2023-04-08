using Project.Domain.Aggregates.Orders.Enums;
using Project.Domain.Aggregates.Orders.ValueObjects;

namespace Project.Domain.Aggregates.Orders.Events
{
    public class OrderStatusChangedEvent : BaseDomainEvent
    {
        public OrderStatusChangedEvent(OrderId orderId, OrderStatus oldValue, OrderStatus newValue)
            : base(orderId.Value)
        {
            OldValue = oldValue.ToString();
            NewValue = newValue.ToString();
        }

        public string OldValue { get; }

        public string NewValue { get; }
    }
}