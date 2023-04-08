using System;
using MediatR;

namespace Project.Application.Aggregates.Orders.Queries.GetOrderById
{
    public class GetOrderByIdQuery : IRequest<OrderQueryResult>
    {
        public Guid Id { get; set; }
    }
}