using MediatR;

namespace Project.Application.Aggregates.Goods.Commands.CreateGood
{
    public class CreateGoodCommand : IRequest<string>
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public decimal Discount { get; set; }

        public bool IsFragile { get; set; }

        public string Description { get; set; }
    }
}