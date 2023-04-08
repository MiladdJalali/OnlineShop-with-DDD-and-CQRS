using Project.Domain.Aggregates.Goods.ValueObjects;

namespace Project.Domain.Aggregates.Goods.Events
{
    public class GoodCreatedEvent : BaseDomainEvent
    {
        public GoodCreatedEvent(GoodId goodId)
            : base(goodId.Value)
        {
        }
    }
}