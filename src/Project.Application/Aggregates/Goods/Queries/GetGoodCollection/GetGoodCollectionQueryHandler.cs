using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Project.Application.Aggregates.Goods.Queries.GetGoodCollection
{
    public sealed class GetGoodCollectionQueryHandler :
        IRequestHandler<GetGoodCollectionQuery, BaseCollectionResult<GoodQueryResult>>
    {
        private readonly IGoodReadRepository goodReadRepository;

        public GetGoodCollectionQueryHandler(IGoodReadRepository goodReadRepository)
        {
            this.goodReadRepository = goodReadRepository;
        }

        public async Task<BaseCollectionResult<GoodQueryResult>> Handle(
            GetGoodCollectionQuery request,
            CancellationToken cancellationToken)
        {
            var result = goodReadRepository.GetAll().OrderBy(i => i.Name);

            var resultWithPaging = await result
                .ApplyPaging(request.PageSize, request.PageIndex)
                .ToArrayAsync(cancellationToken)
                .ConfigureAwait(false);

            return new BaseCollectionResult<GoodQueryResult>
            {
                Result = resultWithPaging,
                TotalCount = resultWithPaging.Length
            };
        }
    }
}