using System;
using MediatR;

namespace Project.Application.Aggregates.Orders.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest<Guid>
    {
        public string[] GoodsName { get; set; }

        public string Description { get; set; }
    }
}