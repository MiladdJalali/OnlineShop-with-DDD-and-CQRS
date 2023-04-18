using System;
using MediatR;

namespace Project.Application.Aggregates.Orders.Commands.ChangeOrderStatus
{
    public class ChangeOrderStatusCommand : IRequest
    {
        public Guid OrderId { get; set; }
        
        public string Status { get; set; }
    }
}