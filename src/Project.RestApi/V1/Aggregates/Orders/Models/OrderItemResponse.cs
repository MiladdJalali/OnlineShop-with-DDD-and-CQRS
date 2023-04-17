using System;

namespace Project.RestApi.V1.Aggregates.Orders.Models
{
    public class OrderItemResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public decimal Discount { get; set; }

        public bool IsFragile { get; set; }

        public string Description { get; set; }
    }
}