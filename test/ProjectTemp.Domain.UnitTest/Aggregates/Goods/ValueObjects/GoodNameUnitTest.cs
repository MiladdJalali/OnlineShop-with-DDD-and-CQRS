using System;
using FluentAssertions;
using Project.Domain.Aggregates.Goods.ValueObjects;
using Project.Domain.Exceptions;
using Project.Domain.Properties;
using Xunit;

namespace Project.Domain.UnitTest.Aggregates.Goods.ValueObjects
{
    public class GoodNameUnitTest
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void TestCreate_WhenValueIsEmpty_ThrowsException(string value)
        {
            var action = new Action(() => GoodName.Create(value));

            action.Should().Throw<DomainException>().WithMessage(DomainResources.Good_GoodNameCannotBeEmpty);
        }

        [Fact]
        public void TestCreate_WhenEverythingIsOk_PropertiesShouldHaveCorrectValues()
        {
            const string value = "GoodName";
            var goodName = GoodName.Create(value);

            goodName.Value.Should().Be(value);
        }

        [Fact]
        public void TestEquality_WhenEverythingIsOk_MustBeTrue()
        {
            const string value = "GoodName";
            var first = GoodName.Create(value);
            var second = GoodName.Create(value);

            first.Should().Be(second);
        }
    }
}