using System;
using System.Threading;
using Moq;
using Project.Application.Aggregates.Goods;
using Project.Application.Aggregates.Orders;
using Project.Application.Aggregates.Orders.Commands.UpdateOrder;
using Project.Application.Services;
using Project.Domain.Aggregates.Orders.Services;
using Project.Domain.UnitTest.Aggregates.Goods.Builders;
using Project.Domain.UnitTest.Aggregates.Orders.Builders;

namespace Project.Application.UnitTest.Aggregates.Orders.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandlerBuilder
    {
        private readonly IUserDescriptor userDescriptor;

        private IGoodWriteRepository goodWriteRepository;

        private IOrderWriteRepository orderWriteRepository;

        public UpdateOrderCommandHandlerBuilder()
        {
            var goodWriteRepositoryMock = new Mock<IGoodWriteRepository>();
            var orderWriteRepositoryMock = new Mock<IOrderWriteRepository>();
            var order = new OrderBuilder().Build();
            var good = new GoodBuilder().Build();

            orderWriteRepositoryMock
                .Setup(i => i.GetById(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(order);

            goodWriteRepositoryMock
                .Setup(i => i.GetByName(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(good);

            orderWriteRepository = orderWriteRepositoryMock.Object;
            goodWriteRepository = goodWriteRepositoryMock.Object;
            userDescriptor = new Mock<IUserDescriptor>().Object;
        }

        public UpdateOrderCommandHandler Build()
        {
            return new UpdateOrderCommandHandler(
                goodWriteRepository,
                orderWriteRepository,
                userDescriptor,
                new Mock<IGoodsTotalPriceValidator>().Object);
        }

        public UpdateOrderCommandHandlerBuilder WithOrderWriteRepository(IOrderWriteRepository value)
        {
            orderWriteRepository = value;
            return this;
        }

        public UpdateOrderCommandHandlerBuilder WithGoodWriteRepository(IGoodWriteRepository value)
        {
            goodWriteRepository = value;
            return this;
        }
    }
}