using System;
using Project.Application.Aggregates.Orders.Commands.UpdateOrder;

namespace Project.Application.UnitTest.Aggregates.Orders.Orders.Commands.UpdateOrder
{
    public static class UpdateOrderCommandBuilder
    {
        public static UpdateOrderCommand Build()
        {
            return new UpdateOrderCommand
            {
                OrderId = Guid.NewGuid(),
                GoodsName = new[] { "UpdatedItemName" },
                Description = "UpdatedOrderDescription"
            };
        }
    }
}