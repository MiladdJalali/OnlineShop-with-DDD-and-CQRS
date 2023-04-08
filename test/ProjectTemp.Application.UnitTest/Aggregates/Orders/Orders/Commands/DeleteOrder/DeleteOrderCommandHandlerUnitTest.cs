using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Project.Application.Aggregates.Orders;
using Project.Application.Properties;
using Project.Domain.Exceptions;
using Xunit;

namespace Project.Application.UnitTest.Aggregates.Orders.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommandHandlerUnitTest
    {
        [Fact]
        public void TestHandle_WhenOrderDoesNotExit_ThrowsException()
        {
            var orderWriteRepositoryMock = new Mock<IOrderWriteRepository>();
            var command = DeleteOrderCommandBuilder.Build();
            var commandHandler = new DeleteOrderCommandHandlerBuilder()
                .WithOrderWriteRepository(orderWriteRepositoryMock.Object)
                .Build();
            var func = new Func<Task>(async () => await commandHandler
                .Handle(command, CancellationToken.None));

            orderWriteRepositoryMock
                .Setup(i => i.GetById(command.Id, CancellationToken.None))
                .ReturnsAsync(() => null);

            func.Should().ThrowAsync<DomainException>().WithMessage(ApplicationResources.Order_UnableToFind);
        }
    }
}