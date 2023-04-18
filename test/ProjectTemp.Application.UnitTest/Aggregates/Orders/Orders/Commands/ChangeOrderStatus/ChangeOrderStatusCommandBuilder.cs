using System;
using Project.Application.Aggregates.Orders.Commands.ChangeOrderStatus;
using Project.Domain.Aggregates.Orders.Enums;

namespace Project.Application.UnitTest.Aggregates.Orders.Orders.Commands.ChangeOrderStatus
{
    public static class ChangeOrderStatusCommandBuilder
    {
        public static ChangeOrderStatusCommand Build()
        {
            return new ChangeOrderStatusCommand
            {
                OrderId = Guid.NewGuid(),
                Status = OrderStatus.Packed.ToString()
            };
        }
    }
}