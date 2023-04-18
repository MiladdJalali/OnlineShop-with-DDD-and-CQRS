using System;
using Project.Application.Aggregates.Orders.Commands.Models;
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
                Goods = new[] { new OrderGoodsCommandModel { Name = "UpdatedItemName", Count = 1 } },
                Description = "UpdatedOrderDescription"
            };
        }
    }
}