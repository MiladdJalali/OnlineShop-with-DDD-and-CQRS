using System;
using MediatR;
using Project.Application.Aggregates.Orders.Commands.Models;

namespace Project.Application.Aggregates.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommand : IRequest
    {
        public Guid OrderId { get; set; }

        public OrderGoodsCommandModel[] Goods { get; set; }

        public string Description { get; set; }
    }
}