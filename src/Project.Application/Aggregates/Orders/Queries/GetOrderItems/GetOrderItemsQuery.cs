using System;
using MediatR;

namespace Project.Application.Aggregates.Orders.Queries.GetOrderItems
{
    public class GetOrderItemsQuery : IRequest<BaseCollectionResult<OrderItemQueryResult>>
    {
        public Guid OrderId { get; set; }
    }
}