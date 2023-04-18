using Project.Application.Aggregates.Orders.Commands.CreateOrder;
using Project.Application.Aggregates.Orders.Commands.Models;

namespace Project.Application.UnitTest.Aggregates.Orders.Orders.Commands.CreateOrder
{
    public static class CreateOrderCommandBuilder
    {
        public static CreateOrderCommand Build()
        {
            return new CreateOrderCommand
            {
                Goods = new[] {new OrderGoodsCommandModel {Name = "ItemName", Count = 1}},
                Description = "Description"
            };
        }
    }
}