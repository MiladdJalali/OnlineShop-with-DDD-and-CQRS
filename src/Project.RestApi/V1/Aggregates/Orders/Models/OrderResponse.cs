using System;

namespace Project.RestApi.V1.Aggregates.Orders.Models
{
    public class OrderResponse
    {
        public Guid Id { get; set; }

        public string Status { get; set; }
        
        public string PostType { get; set; }

        public decimal TotalPrice { get; private set; }

        public string Description { get; set; }

        public string Creator { get; set; }

        public string Updater { get; set; }

        public DateTimeOffset Created { get; set; }

        public DateTimeOffset? Updated { get; set; }
    }
}