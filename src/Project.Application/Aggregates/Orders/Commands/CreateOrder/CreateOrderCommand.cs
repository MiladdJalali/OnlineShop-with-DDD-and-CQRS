using System;
using MediatR;
using Project.Application.Aggregates.Orders.Commands.Models;

namespace Project.Application.Aggregates.Orders.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest<Guid>
    {
        public OrderGoodsCommandModel[] Goods { get; set; }

        public string Description { get; set; }
    }
}