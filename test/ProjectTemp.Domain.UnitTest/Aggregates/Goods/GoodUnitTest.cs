using System;
using FluentAssertions;
using Project.Domain.Aggregates.Goods.Events;
using Project.Domain.Aggregates.Goods.ValueObjects;
using Project.Domain.UnitTest.Aggregates.Goods.Builders;
using Project.Domain.UnitTest.Helpers;
using Project.Domain.ValueObjects;
using Xunit;

namespace Project.Domain.UnitTest.Aggregates.Goods
{
    public class GoodUnitTest
    {
        [Fact]
        public void TestChangeDescription_WhenEverythingIsOk_ValueMustBeSet()
        {
            const string description = "GoodDescription";
            var good = new GoodBuilder().Build();

            good.ClearEvents();
            good.ChangeDescription(Description.Create(description), Guid.NewGuid());

            var descriptionChangedEvent = good.AssertPublishedDomainEvent<GoodDescriptionChangedEvent>();

            good.Description.Value.Should().Be(description);

            descriptionChangedEvent.AggregateId.Should().Be(good.Id.Value);
            descriptionChangedEvent.OldValue.Should().BeNull();
            descriptionChangedEvent.NewValue.Should().Be(description);
        }

        [Fact]
        public void TestChangeDescription_WhenValueIsSame_NothingMustBeHappened()
        {
            const string description = "GoodDescription";
            var good = new GoodBuilder().Build();

            good.ChangeDescription(Description.Create(description), Guid.NewGuid());
            good.ClearEvents();

            good.ChangeDescription(Description.Create(description), Guid.NewGuid());

            good.DomainEvents.Should().BeEmpty();
        }

        [Fact]
        public void TestChangeDescription_WhenEverythingIsOkAndNewValueIsEmpty_ValueMustBeSet()
        {
            const string description = "GoodDescription";
            var good = new GoodBuilder().Build();

            good.ChangeDescription(Description.Create(description), Guid.NewGuid());
            good.ClearEvents();

            good.ChangeDescription(Description.Create(""), Guid.NewGuid());

            var descriptionChangedEvent = good.AssertPublishedDomainEvent<GoodDescriptionChangedEvent>();

            good.DomainEvents.Should().HaveCount(1);
            descriptionChangedEvent.AggregateId.Should().Be(good.Id.Value);
            descriptionChangedEvent.OldValue.Should().Be(description);
            descriptionChangedEvent.NewValue.Should().BeNull();
        }

        [Fact]
        public void TestChangePrice_WhenEverythingIsOk_ValueMustBeSet()
        {
            const decimal oldPrice = 10000;
            const decimal newPrice = 20000;
            var userId = Guid.NewGuid();

            var good = new GoodBuilder()
                .WithPrice(oldPrice)
                .Build();

            good.ClearEvents();
            good.ChangePrice(GoodPrice.Create(newPrice), userId);

            good.UpdaterId.Should().Be(userId);
            good.Price.Value.Should().Be(newPrice);

            var priceChangedEvent = good.AssertPublishedDomainEvent<GoodPriceChangedEvent>();

            priceChangedEvent.AggregateId.Should().Be(good.Id.Value);
            priceChangedEvent.OldValue.Should().Be(oldPrice);
            priceChangedEvent.NewValue.Should().Be(newPrice);
        }

        [Fact]
        public void TestChangePrice_WhenValueIsSame_NothingMustBeHappened()
        {
            const decimal price = 10000;
            var good = new GoodBuilder()
                .WithPrice(price)
                .Build();

            good.ClearEvents();

            good.ChangePrice(GoodPrice.Create(price), Guid.NewGuid());

            good.DomainEvents.Should().BeEmpty();
            good.Price.Value.Should().Be(price);
        }

        [Fact]
        public void TestChangeDiscount_WhenEverythingIsOk_ValueMustBeSet()
        {
            const decimal oldDiscount = 10;
            const decimal newDiscount = 20;
            var userId = Guid.NewGuid();
            var good = new GoodBuilder()
                .WithDiscount(oldDiscount)
                .Build();

            good.ClearEvents();
            good.ChangeDiscount(GoodDiscount.Create(newDiscount), userId);

            good.UpdaterId.Should().Be(userId);
            good.Discount.Value.Should().Be(newDiscount);

            var discountChangedEvent = good.AssertPublishedDomainEvent<GoodDiscountChangedEvent>();

            discountChangedEvent.AggregateId.Should().Be(good.Id.Value);
            discountChangedEvent.OldValue.Should().Be(oldDiscount);
            discountChangedEvent.NewValue.Should().Be(newDiscount);
        }

        [Fact]
        public void TestChangeDiscount_WhenValueIsSame_NothingMustBeHappened()
        {
            const decimal discount = 10;
            var good = new GoodBuilder()
                .WithDiscount(discount)
                .Build();

            good.ClearEvents();

            good.ChangeDiscount(GoodDiscount.Create(discount), Guid.NewGuid());

            good.DomainEvents.Should().BeEmpty();
            good.Discount.Value.Should().Be(discount);
        }

        [Fact]
        public void TestChangeGoodName_WhenEverythingIsOk_ValueMustBeSet()
        {
            const string oldGoodName = "OldGoodName";
            const string newGoodName = "NewGoodName";
            var userId = Guid.NewGuid();
            var good = new GoodBuilder()
                .WithGoodName(oldGoodName)
                .Build();

            good.ClearEvents();
            good.ChangeName(GoodName.Create(newGoodName), userId);
            good.UpdaterId.Should().Be(userId);

            var goodNameChangedEvent = good.AssertPublishedDomainEvent<GoodNameChangedEvent>();

            goodNameChangedEvent.AggregateId.Should().Be(good.Id.Value);
            goodNameChangedEvent.OldValue.Should().Be(oldGoodName);
            goodNameChangedEvent.NewValue.Should().Be(newGoodName);
            good.Name.Value.Should().Be(newGoodName);
        }

        [Fact]
        public void TestChangeGoodName_WhenValueIsSame_NothingMustBeHappened()
        {
            const string name = "goodName";
            var good = new GoodBuilder()
                .WithGoodName(name)
                .Build();

            good.ClearEvents();
            good.ChangeName(GoodName.Create(name), Guid.NewGuid());

            good.DomainEvents.Should().BeEmpty();
        }

        [Fact]
        public void TestChangeIsFragile_WhenEverythingIsOk_ValueMustBeSet()
        {
            var good = new GoodBuilder()
                .WithIsFragile(true)
                .Build();

            good.ClearEvents();
            good.ChangeIsFragile(GoodIsFragile.Create(false), Guid.NewGuid());

            good.IsFragile.Value.Should().BeFalse();

            var isFragileChangedEvent =
                good.AssertPublishedDomainEvent<GoodIsFragileChangedEvent>();

            isFragileChangedEvent.AggregateId.Should().Be(good.Id.Value);
            isFragileChangedEvent.OldValue.Should().BeTrue();
            isFragileChangedEvent.NewValue.Should().BeFalse();
        }

        [Fact]
        public void TestChangeIsFragile_WhenValueIsSame_NothingMustBeHappened()
        {
            var good = new GoodBuilder()
                .WithIsFragile(true)
                .Build();

            good.ClearEvents();
            good.ChangeIsFragile(GoodIsFragile.Create(true), Guid.NewGuid());

            good.IsFragile.Value.Should().BeTrue();
            good.DomainEvents.Should().BeEmpty();
        }

        [Fact]
        public void TestCreate_WhenEverythingIsOk_PropertiesShouldHaveCorrectValues()
        {
            var goodId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var good = new GoodBuilder()
                .WithId(goodId)
                .WithGoodName("goodName")
                .WithPrice(10000)
                .WithDiscount(10)
                .WithIsFragile(true)
                .WithCreatorId(userId)
                .Build();

            var createdEvent = good.AssertPublishedDomainEvent<GoodCreatedEvent>();
            createdEvent.AggregateId.Should().Be(goodId);

            var goodNameChangedEvent = good.AssertPublishedDomainEvent<GoodNameChangedEvent>();
            goodNameChangedEvent.AggregateId.Should().Be(goodId);
            goodNameChangedEvent.OldValue.Should().BeNull();
            goodNameChangedEvent.NewValue.Should().Be("goodName");

            var priceChangedEvent = good.AssertPublishedDomainEvent<GoodPriceChangedEvent>();
            priceChangedEvent.AggregateId.Should().Be(goodId);
            priceChangedEvent.OldValue.Should().BeNull();
            priceChangedEvent.NewValue.Should().Be(10000);

            var discountChangedEvent = good.AssertPublishedDomainEvent<GoodDiscountChangedEvent>();
            discountChangedEvent.AggregateId.Should().Be(goodId);
            discountChangedEvent.OldValue.Should().BeNull();
            discountChangedEvent.NewValue.Should().Be(10);

            var isFragileChangedEvent = good.AssertPublishedDomainEvent<GoodIsFragileChangedEvent>();
            isFragileChangedEvent.AggregateId.Should().Be(goodId);
            isFragileChangedEvent.OldValue.Should().BeNull();
            isFragileChangedEvent.NewValue.Should().BeTrue();

            good.Id.Value.Should().Be(goodId);
            good.Name.Value.Should().Be("goodName");
            good.Price.Value.Should().Be(10000);
            good.Discount.Value.Should().Be(10);
            good.IsFragile.Value.Should().Be(true);
            good.Description.Should().BeNull();
            good.CreatorId.Should().Be(userId);
            good.UpdaterId.Should().Be(userId);
        }

        [Fact]
        public void TestDelete_WhenEverythingIsOk_MustBeMarkedAsDeleted()
        {
            var good = new GoodBuilder().Build();

            good.ClearEvents();
            good.Delete();

            var deletedEvent = good.AssertPublishedDomainEvent<GoodDeletedEvent>();

            deletedEvent.AggregateId.Should().Be(good.Id.Value);

            good.CanBeDeleted().Should().BeTrue();
            good.DomainEvents.Should().HaveCount(1);
        }

        [Fact]
        public void TestDelete_WhenAlreadyDeleted_ThrowsException()
        {
            var good = new GoodBuilder().Build();

            good.Delete();

            var action = new Action(() => good.Delete());
            action.Should().Throw<InvalidOperationException>();
        }
    }
}