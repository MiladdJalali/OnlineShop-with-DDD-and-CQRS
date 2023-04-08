using System;
using Project.Application.Aggregates.Goods.Queries;

namespace Project.Application.Aggregates.Orders.Queries
{
    public class OrderItemQueryResult
    {
        public Guid OrderId { get; set; }

        public OrderQueryResult Order { get; set; }

        public Guid GoodId { get; set; }

        public GoodQueryResult Good { get; set; }

        public string OrderPostType { get; set; }

        public int Count { get; set; }
    }
}