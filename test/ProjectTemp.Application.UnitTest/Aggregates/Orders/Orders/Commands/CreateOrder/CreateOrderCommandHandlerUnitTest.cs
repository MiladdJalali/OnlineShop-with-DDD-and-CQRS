using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Project.Application.Aggregates.Goods;
using Project.Application.Properties;
using Project.Domain.Exceptions;
using Xunit;

namespace Project.Application.UnitTest.Aggregates.Orders.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandHandlerUnitTest
    {
        [Fact]
        public async Task TestHandle_WhenEverythingIsOk_OrderNameMustBeReturned()
        {
            var command = CreateOrderCommandBuilder.Build();
            var commandHandler = new CreateOrderCommandHandlerBuilder().Build();

            var orderId = await commandHandler.Handle(command, CancellationToken.None);

            orderId.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public void TestHandle_WhenProvidedGoodDoesNotExists_ThrowsException()
        {
            var goodWriteRepository = new Mock<IGoodWriteRepository>();
            var command = CreateOrderCommandBuilder.Build();
            var commandHandler = new CreateOrderCommandHandlerBuilder()
                .WithGoodWriteRepository(goodWriteRepository.Object)
                .Build();
            var func = new Func<Task>(async () => await commandHandler
                .Handle(command, CancellationToken.None));

            goodWriteRepository.Setup(i => i.GetByName(command.GoodsName.First(), CancellationToken.None))
                .Returns(() => null);

            func.Should().ThrowAsync<DomainException>()
                .WithMessage(ApplicationResources.Order_UnableToFindGood);
        }
    }
}