using System;
using FluentAssertions;
using Project.Domain.Aggregates.Goods.ValueObjects;
using Project.Domain.Exceptions;
using Project.Domain.Properties;
using Xunit;

namespace Project.Domain.UnitTest.Aggregates.Goods.ValueObjects
{
    public class GoodPriceUnitTest
    {
        [Theory]
        [InlineData(-1)]
        [InlineData(-100)]
        public void TestCreate_WhenInvalidValueProvided_ThrowsException(decimal value)
        {
            var action = new Action(() => GoodPrice.Create(value));

            action.Should().Throw<DomainException>().WithMessage(DomainResources.Good_GoodPriceMustBeValidRule);
        }

        [Fact]
        public void TestCreate_WhenEverythingIsOk_PropertiesShouldHaveCorrectValues()
        {
            var goodPrice = GoodPrice.Create(10000);

            goodPrice.Value.Should().Be(10000);
        }

        [Fact]
        public void TestEquality_WhenEverythingIsOk_MustBeTrue()
        {
            const decimal value = 10000;
            var first = GoodPrice.Create(value);
            var second = GoodPrice.Create(value);

            first.Should().Be(second);
        }
    }
}