using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Project.Application.Aggregates.Goods;
using Project.Application.Aggregates.Orders;
using Project.Application.Aggregates.Orders.Commands.CreateOrder;
using Project.Application.Services;
using Project.Domain.Aggregates.Orders.Services;
using Project.Domain.Aggregates.Orders.ValueObjects;
using Project.Domain.UnitTest.Aggregates.Goods.Builders;

namespace Project.Application.UnitTest.Aggregates.Orders.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandHandlerBuilder
    {
        private readonly IGoodsTotalPriceValidator goodsTotalPriceValidator;

        private readonly IOrderTimeValidator orderTimeValidator;
        private readonly IUserDescriptor userDescriptor;

        private IGoodWriteRepository goodWriteRepository;

        public CreateOrderCommandHandlerBuilder()
        {
            userDescriptor = new Mock<IUserDescriptor>().Object;

            var goodWriteRepositoryMock = new Mock<IGoodWriteRepository>();
            var good = new GoodBuilder().Build();

            goodWriteRepositoryMock
                .Setup(i => i.GetByName(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(good);

            var goodsTotalPriceValidatorMock = new Mock<IGoodsTotalPriceValidator>();
            goodsTotalPriceValidatorMock.Setup(i => i.IsTotalPriceValid(It.IsAny<OrderItem[]>()))
                .Returns(Task.FromResult(true));

            var orderTimeValidatorMock = new Mock<IOrderTimeValidator>();
            orderTimeValidatorMock.Setup(i => i.IsValid(It.IsAny<DateTimeOffset>())).Returns(true);

            goodWriteRepository = goodWriteRepositoryMock.Object;
            goodsTotalPriceValidator = goodsTotalPriceValidatorMock.Object;
            orderTimeValidator = orderTimeValidatorMock.Object;
        }

        public CreateOrderCommandHandler Build()
        {
            return new CreateOrderCommandHandler(
                new Mock<IOrderWriteRepository>().Object,
                goodWriteRepository,
                userDescriptor,
                goodsTotalPriceValidator,
                orderTimeValidator);
        }

        public CreateOrderCommandHandlerBuilder WithGoodWriteRepository(IGoodWriteRepository value)
        {
            goodWriteRepository = value;
            return this;
        }
    }
}