using MediatR;

namespace Project.Application.Aggregates.Goods.Commands.UpdateGood
{
    public class UpdateGoodCommand : IRequest
    {
        public string CurrentName { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public decimal Discount { get; set; }

        public bool IsFragile { get; set; }

        public string Description { get; set; }
    }
}