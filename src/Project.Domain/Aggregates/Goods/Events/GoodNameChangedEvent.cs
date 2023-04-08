using Project.Domain.Aggregates.Goods.ValueObjects;

namespace Project.Domain.Aggregates.Goods.Events
{
    public class GoodNameChangedEvent : BaseDomainEvent
    {
        public GoodNameChangedEvent(GoodId goodId, GoodName oldValue, GoodName newValue)
            : base(goodId.Value)
        {
            OldValue = oldValue?.Value;
            NewValue = newValue.Value;
        }

        public string OldValue { get; }

        public string NewValue { get; }
    }
}