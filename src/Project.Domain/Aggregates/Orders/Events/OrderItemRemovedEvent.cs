using System;
using Project.Domain.Aggregates.Goods.ValueObjects;
using Project.Domain.Aggregates.Orders.ValueObjects;

namespace Project.Domain.Aggregates.Orders.Events
{
    public class OrderItemRemovedEvent : BaseDomainEvent
    {
        public OrderItemRemovedEvent(OrderId orderId, GoodId goodId)
            : base(orderId.Value)
        {
            GoodId = goodId.Value;
        }

        public Guid GoodId { get; }
    }
}