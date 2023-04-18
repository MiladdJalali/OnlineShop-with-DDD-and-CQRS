using System;
using System.Threading;
using Moq;
using Project.Application.Aggregates.Orders;
using Project.Application.Aggregates.Orders.Commands.ChangeOrderStatus;
using Project.Application.Aggregates.Users;
using Project.Application.Services;
using Project.Domain.UnitTest.Aggregates.Orders.Builders;
using Project.Domain.UnitTest.Aggregates.Users.Builders;

namespace Project.Application.UnitTest.Aggregates.Orders.Orders.Commands.ChangeOrderStatus
{
    public class ChangeOrderStatusCommandHandlerBuilder
    {
        private readonly IUserDescriptor userDescriptor;

        private IOrderWriteRepository orderWriteRepository;
        
        private IUserWriteRepository userWriteRepository;

        public ChangeOrderStatusCommandHandlerBuilder()
        {
            var orderWriteRepositoryMock = new Mock<IOrderWriteRepository>();
            var order = new OrderBuilder().Build();
            var userWriteRepositoryMock = new Mock<IUserWriteRepository>();
            var user = new UserBuilder().Build();

            orderWriteRepositoryMock
                .Setup(i => i.GetById(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(order);

            userWriteRepositoryMock
                .Setup(i => i.GetById(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(user);

            orderWriteRepository = orderWriteRepositoryMock.Object;
            userWriteRepository = userWriteRepositoryMock.Object;
            userDescriptor = new Mock<IUserDescriptor>().Object;
        }

        public ChangeOrderStatusCommandHandler Build()
        {
            return new ChangeOrderStatusCommandHandler(orderWriteRepository, userWriteRepository, userDescriptor);
        }

        public ChangeOrderStatusCommandHandlerBuilder WithOrderWriteRepository(IOrderWriteRepository value)
        {
            orderWriteRepository = value;
            return this;
        }

        public ChangeOrderStatusCommandHandlerBuilder WithUserWriteRepository(IUserWriteRepository value)
        {
            userWriteRepository = value;
            return this;
        }
    }
}