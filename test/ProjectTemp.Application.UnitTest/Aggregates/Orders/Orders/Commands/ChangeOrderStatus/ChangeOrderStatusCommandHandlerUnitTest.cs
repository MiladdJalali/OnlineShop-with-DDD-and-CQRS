using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Project.Application.Aggregates.Orders;
using Project.Application.Aggregates.Users;
using Project.Application.Properties;
using Project.Domain.Aggregates.Orders.Enums;
using Project.Domain.Exceptions;
using Project.Domain.UnitTest.Aggregates.Orders.Builders;
using Xunit;

namespace Project.Application.UnitTest.Aggregates.Orders.Orders.Commands.ChangeOrderStatus
{
    public class ChangeOrderStatusCommandHandlerUnitTest
    {
        [Fact]
        public void TestHandle_WhenOrderDoesNotExit_ThrowsException()
        {
            var orderWriteRepositoryMock = new Mock<IOrderWriteRepository>();
            var command = ChangeOrderStatusCommandBuilder.Build();
            var commandHandler = new ChangeOrderStatusCommandHandlerBuilder()
                .WithOrderWriteRepository(orderWriteRepositoryMock.Object)
                .Build();
            var func = new Func<Task>(async () => await commandHandler
                .Handle(command, CancellationToken.None));

            orderWriteRepositoryMock
                .Setup(i => i.GetById(command.OrderId, CancellationToken.None))
                .ReturnsAsync(() => null);

            func.Should().ThrowAsync<DomainException>()
                .WithMessage(ApplicationResources.Order_UnableToFind);
        }

        [Fact]
        public void TestHandle_WhenUserDoesNotExit_ThrowsException()
        {
            var userWriteRepositoryMock = new Mock<IUserWriteRepository>();
            var command = ChangeOrderStatusCommandBuilder.Build();
            var commandHandler = new ChangeOrderStatusCommandHandlerBuilder()
                .WithUserWriteRepository(userWriteRepositoryMock.Object)
                .Build();
            var func = new Func<Task>(async () => await commandHandler
                .Handle(command, CancellationToken.None));

            userWriteRepositoryMock
                .Setup(i => i.GetById(command.OrderId, CancellationToken.None))
                .ReturnsAsync(() => null);

            func.Should().ThrowAsync<DomainException>()
                .WithMessage(ApplicationResources.Order_UserDoesNotExist);
        }

        [Fact]
        public void TestHandle_WhenOrderIsDelivered_ThrowsException()
        {
            var orderWriteRepositoryMock = new Mock<IOrderWriteRepository>();
            var command = ChangeOrderStatusCommandBuilder.Build();
            var commandHandler = new ChangeOrderStatusCommandHandlerBuilder()
                .WithOrderWriteRepository(orderWriteRepositoryMock.Object)
                .Build();
            var func = new Func<Task>(async () => await commandHandler
                .Handle(command, CancellationToken.None));

            var deliveredOrder = new OrderBuilder().Build();
            deliveredOrder.ChangeStatus(OrderStatus.Delivered, Guid.NewGuid());

            orderWriteRepositoryMock
                .Setup(i => i.GetById(command.OrderId, CancellationToken.None))
                .ReturnsAsync(() => deliveredOrder);

            func.Should().ThrowAsync<DomainException>()
                .WithMessage(ApplicationResources.Order_DeliveredOrderStatusCanNotBeChanged);
        }
    }
}