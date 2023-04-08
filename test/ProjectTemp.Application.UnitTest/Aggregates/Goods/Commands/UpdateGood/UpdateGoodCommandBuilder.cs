using Project.Application.Aggregates.Goods.Commands.UpdateGood;

namespace Project.Application.UnitTest.Aggregates.Goods.Commands.UpdateGood
{
    public static class UpdateGoodCommandBuilder
    {
        public static UpdateGoodCommand Build()
        {
            return new UpdateGoodCommand
            {
                CurrentName = "Name",
                Name = "UpdatedGoodName",
                Description = "UpdatedGoodDescription"
            };
        }
    }
}