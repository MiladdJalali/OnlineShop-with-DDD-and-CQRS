using Project.Application.Aggregates.Goods.Commands.CreateGood;

namespace Project.Application.UnitTest.Aggregates.Goods.Commands.CreateGood
{
    public static class CreateGoodCommandBuilder
    {
        public static CreateGoodCommand Build()
        {
            return new CreateGoodCommand
            {
                Name = "Name",
                Price = 10000,
                Discount = 10,
                Description = "Description"
            };
        }
    }
}