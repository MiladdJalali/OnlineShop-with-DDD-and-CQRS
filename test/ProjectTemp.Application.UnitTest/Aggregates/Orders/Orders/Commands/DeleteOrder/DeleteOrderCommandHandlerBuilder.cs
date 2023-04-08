using System;
using System.Threading;
using Moq;
using Project.Application.Aggregates.Orders;
using Project.Application.Aggregates.Orders.Commands.DeleteOrder;
using Project.Domain.UnitTest.Aggregates.Orders.Builders;

namespace Project.Application.UnitTest.Aggregates.Orders.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommandHandlerBuilder
    {
        private IOrderWriteRepository orderWriteRepository;

        public DeleteOrderCommandHandlerBuilder()
        {
            var orderWriteRepositoryMock = new Mock<IOrderWriteRepository>();
            var order = new OrderBuilder().Build();

            orderWriteRepositoryMock
                .Setup(i => i.GetById(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(order);

            orderWriteRepository = orderWriteRepositoryMock.Object;
        }

        public DeleteOrderCommandHandler Build()
        {
            return new DeleteOrderCommandHandler(orderWriteRepository);
        }

        public DeleteOrderCommandHandlerBuilder WithOrderWriteRepository(IOrderWriteRepository value)
        {
            orderWriteRepository = value;
            return this;
        }
    }
}