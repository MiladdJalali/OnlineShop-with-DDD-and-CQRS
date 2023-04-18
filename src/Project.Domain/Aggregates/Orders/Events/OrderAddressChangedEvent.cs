using Project.Domain.Aggregates.Orders.ValueObjects;
using Project.Domain.Aggregates.Users.ValueObjects;

namespace Project.Domain.Aggregates.Orders.Events
{
    public class OrderAddressChangedEvent : BaseDomainEvent
    {
        public OrderAddressChangedEvent(OrderId orderId, UserAddress oldValue, UserAddress newValue)
            : base(orderId.Value)
        {
            OldValue = oldValue?.Value;
            NewValue = newValue?.Value;
        }

        public string OldValue { get; }

        public string NewValue { get; }
    }
}