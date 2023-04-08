using MediatR;

namespace Project.Application.Aggregates.Orders.Queries.GetOrderCollection
{
    public class GetOrderCollectionQuery :
        BaseCollectionQuery, IRequest<BaseCollectionResult<OrderQueryResult>>
    {
    }
}