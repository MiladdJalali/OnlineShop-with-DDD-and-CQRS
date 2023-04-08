using Project.Domain.Aggregates.Goods.ValueObjects;

namespace Project.Domain.Aggregates.Goods.Events
{
    public class GoodDiscountChangedEvent : BaseDomainEvent
    {
        public GoodDiscountChangedEvent(GoodId goodId, GoodDiscount oldValue, GoodDiscount newValue)
            : base(goodId.Value)
        {
            OldValue = oldValue?.Value;
            NewValue = newValue.Value;
        }

        public decimal? OldValue { get; }

        public decimal NewValue { get; }
    }
}