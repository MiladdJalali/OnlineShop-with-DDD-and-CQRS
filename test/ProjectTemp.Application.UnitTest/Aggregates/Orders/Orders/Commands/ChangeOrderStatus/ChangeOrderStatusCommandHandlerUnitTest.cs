using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Project.Application.Aggregates.Orders;
using Project.Application.Aggregates.Orders.Commands.ChangeOrderStatus;
using Project.Application.Properties;
using Project.Domain.Exceptions;
using Xunit;

namespace Project.Application.UnitTest.Aggregates.Orders.Orders.Commands.ChangeOrderStatus
{
    public class ChangeOrderStatusCommandHandlerUnitTest
    {
        [Fact]
        public void TestHandle_WhenInvalidStatusProvided_ThrowsException()
        {
            var command = new ChangeOrderStatusCommand
            {
                OrderId = Guid.NewGuid(),
                Status = "InvalidValue"
            };

            var commandHandler = new ChangeOrderStatusCommandHandlerBuilder().Build();
            var func = new Func<Task>(async () => await commandHandler.Handle(command, CancellationToken.None));

            func.Should().ThrowAsync<DomainException>()
                .WithMessage(ApplicationResources.Order_InvalidStatusProvided);
        }

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
    }
}