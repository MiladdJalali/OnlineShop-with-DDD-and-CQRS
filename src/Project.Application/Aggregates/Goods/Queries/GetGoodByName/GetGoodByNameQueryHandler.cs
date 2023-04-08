using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Project.Application.Aggregates.Goods.Queries.GetGoodByName
{
    public sealed class GetGoodByNameQueryHandler : IRequestHandler<GetGoodByNameQuery, GoodQueryResult>
    {
        private readonly IGoodReadRepository goodReadRepository;

        public GetGoodByNameQueryHandler(IGoodReadRepository goodReadRepository)
        {
            this.goodReadRepository = goodReadRepository;
        }

        public Task<GoodQueryResult> Handle(
            GetGoodByNameQuery request,
            CancellationToken cancellationToken)
        {
            return goodReadRepository.GetByName(request.Name, cancellationToken);
        }
    }
}