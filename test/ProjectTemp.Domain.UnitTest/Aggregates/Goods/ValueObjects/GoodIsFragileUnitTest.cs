using FluentAssertions;
using Project.Domain.Aggregates.Goods.ValueObjects;
using Xunit;

namespace Project.Domain.UnitTest.Aggregates.Goods.ValueObjects
{
    public class GoodIsFragileUnitTest
    {
        [Fact]
        public void TestCreate_WhenEverythingIsOk_ValueMustBeSet()
        {
            var goodIsFragile = GoodIsFragile.Create(false);

            goodIsFragile.Value.Should().BeFalse();
        }

        [Fact]
        public void TestEquality_WhenEverythingIsOk_MustBeTrue()
        {
            var first = GoodIsFragile.Create(true);
            var second = GoodIsFragile.Create(true);

            first.Should().Be(second);
        }
    }
}