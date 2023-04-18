using System;
using Project.Domain.Aggregates.Goods.ValueObjects;
using Project.Domain.Aggregates.Orders.ValueObjects;

namespace Project.Domain.Aggregates.Orders.Events
{
    public class OrderItemAddedEvent : BaseDomainEvent
    {
        public OrderItemAddedEvent(OrderId orderId, GoodId goodId, int count)
            : base(orderId.Value)
        {
            GoodId = goodId.Value;
            Count = count;
        }

        public Guid GoodId { get; }

        public int Count { get; }
    }
}