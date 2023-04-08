using Project.Application.Aggregates.Orders.Commands.CreateOrder;

namespace Project.Application.UnitTest.Aggregates.Orders.Orders.Commands.CreateOrder
{
    public static class CreateOrderCommandBuilder
    {
        public static CreateOrderCommand Build()
        {
            return new CreateOrderCommand
            {
                GoodsName = new[] {"ItemName"},
                Description = "Description"
            };
        }
    }
}