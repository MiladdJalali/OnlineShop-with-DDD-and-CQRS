using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Project.Application.Aggregates.Orders.Queries.GetOrderCollection
{
    public sealed class GetOrderCollectionQueryHandler :
        IRequestHandler<GetOrderCollectionQuery, BaseCollectionResult<OrderQueryResult>>
    {
        private readonly IOrderReadRepository orderReadRepository;

        public GetOrderCollectionQueryHandler(IOrderReadRepository orderReadRepository)
        {
            this.orderReadRepository = orderReadRepository;
        }

        public async Task<BaseCollectionResult<OrderQueryResult>> Handle(
            GetOrderCollectionQuery request,
            CancellationToken cancellationToken)
        {
            var result = orderReadRepository.GetAll();

            var resultWithPaging = await result
                .ApplyPaging(request.PageSize, request.PageIndex)
                .ToArrayAsync(cancellationToken)
                .ConfigureAwait(false);

            return new BaseCollectionResult<OrderQueryResult>
            {
                Result = resultWithPaging,
                TotalCount = resultWithPaging.Length
            };
        }
    }
}