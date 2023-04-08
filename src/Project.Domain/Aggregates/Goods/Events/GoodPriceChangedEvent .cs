using Project.Domain.Aggregates.Goods.ValueObjects;

namespace Project.Domain.Aggregates.Goods.Events
{
    public class GoodPriceChangedEvent : BaseDomainEvent
    {
        public GoodPriceChangedEvent(GoodId goodId, GoodPrice oldValue, GoodPrice newValue)
            : base(goodId.Value)
        {
            OldValue = oldValue?.Value;
            NewValue = newValue.Value;
        }

        public decimal? OldValue { get; }

        public decimal NewValue { get; }
    }
}