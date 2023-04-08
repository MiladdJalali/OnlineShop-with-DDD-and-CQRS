using System;
using FluentAssertions;
using Project.Domain.Aggregates.Users.ValueObjects;
using Project.Domain.Exceptions;
using Project.Domain.Properties;
using Xunit;

namespace Project.Domain.UnitTest.Aggregates.Users.ValueObjects
{
    public class UserAddressUnitTest
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void TestCreate_WhenValueIsEmpty_ThrowsException(string value)
        {
            var action = new Action(() => UserAddress.Create(value));

            action.Should().Throw<DomainException>().WithMessage(DomainResources.User_AddressCannotBeEmpty);
        }

        [Fact]
        public void TestCreate_WhenEverythingIsOk_PropertiesShouldHaveCorrectValues()
        {
            const string value = "UserAddress";
            var address = UserAddress.Create(value);

            address.Value.Should().Be(value);
        }

        [Fact]
        public void TestEquality_WhenEverythingIsOk_MustBeTrue()
        {
            const string value = "UserAddress";
            var first = UserAddress.Create(value);
            var second = UserAddress.Create(value);

            first.Should().Be(second);
        }
    }
}