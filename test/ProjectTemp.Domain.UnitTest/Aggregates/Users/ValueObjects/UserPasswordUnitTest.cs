﻿using System;
using FluentAssertions;
using Project.Domain.Aggregates.Users.ValueObjects;
using Project.Domain.Exceptions;
using Project.Domain.Properties;
using Xunit;

namespace Project.Domain.UnitTest.Aggregates.Users.ValueObjects
{
    public class UserPasswordUnitTest
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void TestCreate_WhenValueIsEmpty_ThrowsException(string value)
        {
            var action = new Action(() => UserPassword.Create(value));

            action.Should().Throw<DomainException>().WithMessage(DomainResources.User_PasswordCannotBeEmpty);
        }

        [Fact]
        public void TestCreate_WhenEverythingIsOk_PropertiesShouldHaveCorrectValues()
        {
            const string value = "UserPassword";
            var password = UserPassword.Create(value);

            password.Value.Should().Be(value);
        }

        [Fact]
        public void TestEquality_WhenEverythingIsOk_MustBeTrue()
        {
            const string value = "UserPassword";
            var first = UserPassword.Create(value);
            var second = UserPassword.Create(value);

            first.Should().Be(second);
        }
    }
}