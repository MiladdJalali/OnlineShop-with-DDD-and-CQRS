using MediatR;

namespace Project.Application.Aggregates.Goods.Queries.GetGoodCollection
{
    public class GetGoodCollectionQuery :
        BaseCollectionQuery, IRequest<BaseCollectionResult<GoodQueryResult>>
    {
    }
}