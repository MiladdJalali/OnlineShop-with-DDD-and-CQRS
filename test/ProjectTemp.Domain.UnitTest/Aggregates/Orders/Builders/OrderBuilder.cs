using System;
using Moq;
using Project.Domain.Aggregates.Orders;
using Project.Domain.Aggregates.Orders.Services;
using Project.Domain.Aggregates.Orders.ValueObjects;

namespace Project.Domain.UnitTest.Aggregates.Orders.Builders
{
    public class OrderBuilder
    {
        private Guid creatorId;

        private Guid id;

        private IOrderTimeValidator validator;

        public OrderBuilder()
        {
            id = Guid.NewGuid();
            creatorId = Guid.NewGuid();
            var mock = new Mock<IOrderTimeValidator>();
            mock.Setup(i => i.IsValid(It.IsAny<DateTimeOffset>())).Returns(true);
            validator = mock.Object;
        }

        public Order Build()
        {
            return Order.Create(
                OrderId.Create(id),
                creatorId,
                validator);
        }

        public OrderBuilder WithId(Guid value)
        {
            id = value;
            return this;
        }

        public OrderBuilder WithCreatorId(Guid value)
        {
            creatorId = value;
            return this;
        }

        public OrderBuilder WithOrderTimeValidator(IOrderTimeValidator value)
        {
            validator = value;
            return this;
        }
    }
}