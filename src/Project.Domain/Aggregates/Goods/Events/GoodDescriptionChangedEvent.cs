using Project.Domain.Aggregates.Goods.ValueObjects;
using Project.Domain.ValueObjects;

namespace Project.Domain.Aggregates.Goods.Events
{
    public class GoodDescriptionChangedEvent : BaseDomainEvent
    {
        public GoodDescriptionChangedEvent(GoodId goodId, Description oldValue, Description newValue)
            : base(goodId.Value)
        {
            OldValue = oldValue?.Value;
            NewValue = newValue?.Value;
        }

        public string OldValue { get; }

        public string NewValue { get; }
    }
}