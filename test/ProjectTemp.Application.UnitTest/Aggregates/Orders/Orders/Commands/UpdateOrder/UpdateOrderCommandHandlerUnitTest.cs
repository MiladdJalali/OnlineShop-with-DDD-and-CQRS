using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Project.Application.Aggregates.Goods;
using Project.Application.Aggregates.Orders;
using Project.Application.Properties;
using Project.Domain.Exceptions;
using Xunit;

namespace Project.Application.UnitTest.Aggregates.Orders.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandlerUnitTest
    {
        [Fact]
        public void TestHandle_WhenOrderItemDoesNotExist_ThrowsException()
        {
            var goodWriteRepository = new Mock<IGoodWriteRepository>();
            var command = UpdateOrderCommandBuilder.Build();
            var commandHandler = new UpdateOrderCommandHandlerBuilder()
                .WithGoodWriteRepository(goodWriteRepository.Object)
                .Build();
            var func = new Func<Task>(async () => await commandHandler
                .Handle(command, CancellationToken.None));

            goodWriteRepository.Setup(i =>
                i.GetByName(command.Goods.First().Name, CancellationToken.None)).Returns(() => null);

            func.Should().ThrowAsync<DomainException>()
                .WithMessage(ApplicationResources.Order_UnableToFindGood);
        }

        [Fact]
        public void TestHandle_WhenOrderDoesNotExit_ThrowsException()
        {
            var orderWriteRepositoryMock = new Mock<IOrderWriteRepository>();
            var command = UpdateOrderCommandBuilder.Build();
            var commandHandler = new UpdateOrderCommandHandlerBuilder()
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