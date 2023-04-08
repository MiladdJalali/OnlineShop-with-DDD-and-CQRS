using Project.Domain.Aggregates.Goods.ValueObjects;

namespace Project.Domain.Aggregates.Goods.Events
{
    public class GoodIsFragileChangedEvent : BaseDomainEvent
    {
        public GoodIsFragileChangedEvent(GoodId goodId, GoodIsFragile oldValue, GoodIsFragile newValue)
            : base(goodId.Value)
        {
            OldValue = oldValue?.Value;
            NewValue = newValue.Value;
        }

        public bool? OldValue { get; }

        public bool NewValue { get; }
    }
}