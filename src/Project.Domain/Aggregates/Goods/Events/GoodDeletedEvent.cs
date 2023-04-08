using Project.Domain.Aggregates.Goods.ValueObjects;

namespace Project.Domain.Aggregates.Goods.Events
{
    public class GoodDeletedEvent : BaseDomainEvent
    {
        public GoodDeletedEvent(GoodId goodId)
            : base(goodId.Value)
        {
        }
    }
}