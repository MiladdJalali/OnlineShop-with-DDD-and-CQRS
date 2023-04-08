using System;
using MediatR;

namespace Project.Application.Aggregates.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommand : IRequest
    {
        public Guid OrderId { get; set; }

        public string[] GoodsName { get; set; }

        public string Description { get; set; }
    }
}