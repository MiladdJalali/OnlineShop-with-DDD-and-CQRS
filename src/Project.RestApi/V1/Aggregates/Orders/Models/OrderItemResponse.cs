using System;
using Project.RestApi.V1.Aggregates.Goods.Models;

namespace Project.RestApi.V1.Aggregates.Orders.Models
{
    public class OrderItemResponse
    {
        public Guid OrderId { get; set; }

        public Guid GoodId { get; set; }

        public GoodResponse Good { get; private init; }

        public string OrderPostType { get; private init; }

        public int Count { get; private init; }
    }
}