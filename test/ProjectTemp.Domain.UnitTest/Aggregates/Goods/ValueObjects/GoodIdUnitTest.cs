﻿using System;
using FluentAssertions;
using Project.Domain.Aggregates.Goods.ValueObjects;
using Xunit;

namespace Project.Domain.UnitTest.Aggregates.Goods.ValueObjects
{
    public class GoodIdUnitTest
    {
        [Fact]
        public void TestCreate_WhenEverythingIsOk_PropertiesShouldHaveCorrectValues()
        {
            var value = Guid.NewGuid();
            var goodId = GoodId.Create(value);

            goodId.Value.Should().Be(value);
        }

        [Theory]
        [InlineData("37ABBF87-A96D-4593-A0C4-23FEC62D6559", true)]
        [InlineData("CD8BE2BC-982D-4258-9A2F-3AE3D967AA76", false)]
        public void TestEquality_WhenEverythingIsOk_ResultMustBeExpected(string value, bool result)
        {
            var first = GoodId.Create(Guid.Parse("37ABBF87-A96D-4593-A0C4-23FEC62D6559"));
            var second = GoodId.Create(Guid.Parse(value));

            first.Equals(second).Should().Be(result);
        }
    }
}