using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Project.Application.Aggregates.Orders.Queries.GetOrderItems
{
    public sealed class GetOrderItemsQueryHandler :
        IRequestHandler<GetOrderItemsQuery, BaseCollectionResult<OrderItemQueryResult>>
    {
        private readonly IOrderReadRepository orderReadRepository;

        public GetOrderItemsQueryHandler(IOrderReadRepository orderReadRepository)
        {
            this.orderReadRepository = orderReadRepository;
        }

        public async Task<BaseCollectionResult<OrderItemQueryResult>> Handle(
            GetOrderItemsQuery request,
            CancellationToken cancellationToken)
        {
            var result = await orderReadRepository
                .GetOrderItems(request.OrderId, cancellationToken)
                .ConfigureAwait(false);

            return new BaseCollectionResult<OrderItemQueryResult>
            {
                Result = result,
                TotalCount = result.Length
            };
        }
    }
}