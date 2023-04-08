using System;
using Project.Application.Aggregates.Orders.Commands.DeleteOrder;

namespace Project.Application.UnitTest.Aggregates.Orders.Orders.Commands.DeleteOrder
{
    public static class DeleteOrderCommandBuilder
    {
        public static DeleteOrderCommand Build()
        {
            return new DeleteOrderCommand
            {
                Id = Guid.NewGuid()
            };
        }
    }
}