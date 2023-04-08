using Project.Application.Aggregates.Goods.Commands.DeleteGood;

namespace Project.Application.UnitTest.Aggregates.Goods.Commands.DeleteGood
{
    public static class DeleteGoodCommandBuilder
    {
        public static DeleteGoodCommand Build()
        {
            return new DeleteGoodCommand
            {
                Name = "Name"
            };
        }
    }
}