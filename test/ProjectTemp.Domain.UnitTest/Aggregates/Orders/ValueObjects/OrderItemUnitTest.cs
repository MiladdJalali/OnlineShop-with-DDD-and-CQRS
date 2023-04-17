using System;
using FluentAssertions;
using Project.Domain.Aggregates.Goods.ValueObjects;
using Project.Domain.Aggregates.Orders.ValueObjects;
using Project.Domain.Exceptions;
using Project.Domain.Properties;
using Xunit;

namespace Project.Domain.UnitTest.Aggregates.Orders.ValueObjects
{
    public class OrderItemUnitTest
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10)]
        public void TestCreate_WhenCountIsInvalid_ThrowsException(int value)
        {
            var action = new Action(() =>
                OrderItem.Create(GoodId.Create(Guid.NewGuid()), value));

            action.Should().Throw<DomainException>().WithMessage(DomainResources.Order_CountMustBeAtleastOne);
        }

        [Fact]
        public void TestEquality_WhenEverythingIsOk_MustBeTrue()
        {
            var goodId = GoodId.Create(Guid.NewGuid());
            var isFragile = GoodIsFragile.Create(true);
            const int count = 5;

            var first = OrderItem.Create(goodId, count);
            var second = OrderItem.Create(goodId, count);

            first.Should().Be(second);
        }
    }
}