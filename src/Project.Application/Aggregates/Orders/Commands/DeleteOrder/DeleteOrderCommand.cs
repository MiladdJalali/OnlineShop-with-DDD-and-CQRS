using System;
using MediatR;

namespace Project.Application.Aggregates.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}