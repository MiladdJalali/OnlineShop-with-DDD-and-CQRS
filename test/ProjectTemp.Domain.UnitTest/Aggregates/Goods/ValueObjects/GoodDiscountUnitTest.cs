using System;
using FluentAssertions;
using Project.Domain.Aggregates.Goods.ValueObjects;
using Project.Domain.Exceptions;
using Project.Domain.Properties;
using Xunit;

namespace Project.Domain.UnitTest.Aggregates.Goods.ValueObjects
{
    public class GoodDiscountUnitTest
    {
        [Theory]
        [InlineData(-1)]
        [InlineData(-10)]
        [InlineData(101)]
        [InlineData(110)]
        public void TestCreate_WhenInvalidValueProvided_ThrowsException(decimal value)
        {
            var action = new Action(() => GoodDiscount.Create(value));

            action.Should().Throw<DomainException>().WithMessage(DomainResources.Good_GoodDiscountMustBeValidRule);
        }

        [Fact]
        public void TestCreate_WhenEverythingIsOk_PropertiesShouldHaveCorrectValues()
        {
            var goodDiscount = GoodDiscount.Create(10);

            goodDiscount.Value.Should().Be(10);
        }

        [Fact]
        public void TestEquality_WhenEverythingIsOk_MustBeTrue()
        {
            const decimal value = 10;
            var first = GoodDiscount.Create(value);
            var second = GoodDiscount.Create(value);

            first.Should().Be(second);
        }
    }
}