using MediatR;

namespace Project.Application.Aggregates.Goods.Queries.GetGoodByName
{
    public class GetGoodByNameQuery : IRequest<GoodQueryResult>
    {
        public string Name { get; set; }
    }
}