using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Project.Application.Aggregates.Orders.Queries.GetOrderById
{
    public sealed class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderQueryResult>
    {
        private readonly IOrderReadRepository orderReadRepository;

        public GetOrderByIdQueryHandler(IOrderReadRepository orderReadRepository)
        {
            this.orderReadRepository = orderReadRepository;
        }

        public Task<OrderQueryResult> Handle(
            GetOrderByIdQuery request,
            CancellationToken cancellationToken)
        {
            return orderReadRepository.GetById(request.Id, cancellationToken);
        }
    }
}