using Project.Domain.Aggregates.Orders.ValueObjects;
using Project.Domain.ValueObjects;

namespace Project.Domain.Aggregates.Orders.Events
{
    public class OrderDescriptionChangedEvent : BaseDomainEvent
    {
        public OrderDescriptionChangedEvent(OrderId orderId, Description oldValue, Description newValue)
            : base(orderId.Value)
        {
            OldValue = oldValue?.Value;
            NewValue = newValue?.Value;
        }

        public string OldValue { get; }

        public string NewValue { get; }
    }
}