using MediatR;

namespace Project.Application.Aggregates.Goods.Commands.DeleteGood
{
    public class DeleteGoodCommand : IRequest
    {
        public string Name { get; set; }
    }
}