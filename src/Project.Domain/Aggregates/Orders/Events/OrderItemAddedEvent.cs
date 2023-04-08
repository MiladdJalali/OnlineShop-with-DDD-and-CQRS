using System;
using Project.Domain.Aggregates.Goods.ValueObjects;
using Project.Domain.Aggregates.Orders.Enums;
using Project.Domain.Aggregates.Orders.ValueObjects;

namespace Project.Domain.Aggregates.Orders.Events
{
    public class OrderItemAddedEvent : BaseDomainEvent
    {
        public OrderItemAddedEvent(OrderId orderId, GoodId goodId, OrderPostType postType, int count)
            : base(orderId.Value)
        {
            GoodId = goodId.Value;
            PostType = postType.ToString();
            Count = count;
        }

        public Guid GoodId { get; }

        public string PostType { get; }

        public int Count { get; }
    }
}