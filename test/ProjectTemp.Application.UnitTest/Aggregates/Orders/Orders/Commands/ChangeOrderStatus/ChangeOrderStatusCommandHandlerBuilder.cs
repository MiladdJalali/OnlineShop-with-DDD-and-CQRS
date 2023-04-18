using System;
using System.Threading;
using Moq;
using Project.Application.Aggregates.Orders;
using Project.Application.Aggregates.Orders.Commands.ChangeOrderStatus;
using Project.Application.Services;
using Project.Domain.UnitTest.Aggregates.Orders.Builders;

namespace Project.Application.UnitTest.Aggregates.Orders.Orders.Commands.ChangeOrderStatus
{
    public class ChangeOrderStatusCommandHandlerBuilder
    {
        private readonly IUserDescriptor userDescriptor;

        private IOrderWriteRepository orderWriteRepository;

        public ChangeOrderStatusCommandHandlerBuilder()
        {
            var orderWriteRepositoryMock = new Mock<IOrderWriteRepository>();
            var order = new OrderBuilder().Build();

            orderWriteRepositoryMock
                .Setup(i => i.GetById(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(order);

            orderWriteRepository = orderWriteRepositoryMock.Object;
            userDescriptor = new Mock<IUserDescriptor>().Object;
        }

        public ChangeOrderStatusCommandHandler Build()
        {
            return new ChangeOrderStatusCommandHandler(orderWriteRepository, userDescriptor);
        }

        public ChangeOrderStatusCommandHandlerBuilder WithOrderWriteRepository(IOrderWriteRepository value)
        {
            orderWriteRepository = value;
            return this;
        }
    }
}