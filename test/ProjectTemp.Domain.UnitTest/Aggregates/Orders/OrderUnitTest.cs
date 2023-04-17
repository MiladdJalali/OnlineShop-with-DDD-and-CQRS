using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Project.Domain.Aggregates.Goods.ValueObjects;
using Project.Domain.Aggregates.Orders.Enums;
using Project.Domain.Aggregates.Orders.Events;
using Project.Domain.Aggregates.Orders.Services;
using Project.Domain.Aggregates.Orders.ValueObjects;
using Project.Domain.Exceptions;
using Project.Domain.UnitTest.Aggregates.Orders.Builders;
using Project.Domain.UnitTest.Helpers;
using Project.Domain.ValueObjects;
using Xunit;

namespace Project.Domain.UnitTest.Aggregates.Orders
{
    public class OrderUnitTest
    {
        [Fact]
        public void TestCreate_WhenEverythingIsOk_PropertiesShouldHaveCorrectValues()
        {
            var orderId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var order = new OrderBuilder()
                .WithId(orderId)
                .WithCreatorId(userId)
                .Build();

            var createdEvent = order.AssertPublishedDomainEvent<OrderCreatedEvent>();
            createdEvent.AggregateId.Should().Be(orderId);
            createdEvent.Status.Should().Be(OrderStatus.Received.ToString());

            createdEvent.AggregateId.Should().Be(orderId);
            createdEvent.Status.Should().Be(OrderStatus.Received.ToString());

            order.Id.Value.Should().Be(orderId);
            order.Status.Should().Be(OrderStatus.Received);
            order.Description.Should().BeNull();
            order.CreatorId.Should().Be(userId);
        }

        [Fact]
        public void TestCreate_WhenOrderTimeIsNoValid_ThrowsException()
        {
            var validator = new Mock<IOrderTimeValidator>();
            validator.Setup(i => i.IsValid(It.IsAny<DateTimeOffset>())).Returns(false);

            Action action = () => { new OrderBuilder().WithOrderTimeValidator(validator.Object).Build(); };
            action.Should().Throw<DomainException>();
        }

        [Fact]
        public void TestChangeDescription_WhenEverythingIsOk_ValueMustBeSet()
        {
            const string description = "OrderDescription";
            var order = new OrderBuilder().Build();

            order.ClearEvents();
            order.ChangeDescription(Description.Create(description), Guid.NewGuid());

            var descriptionChangedEvent = order.AssertPublishedDomainEvent<OrderDescriptionChangedEvent>();

            order.Description.Value.Should().Be(description);

            descriptionChangedEvent.AggregateId.Should().Be(order.Id.Value);
            descriptionChangedEvent.OldValue.Should().BeNull();
            descriptionChangedEvent.NewValue.Should().Be(description);
        }

        [Fact]
        public void TestChangeDescription_WhenValueIsSame_NothingMustBeHappened()
        {
            const string description = "OrderDescription";
            var order = new OrderBuilder().Build();

            order.ChangeDescription(Description.Create(description), Guid.NewGuid());
            order.ClearEvents();

            order.ChangeDescription(Description.Create(description), Guid.NewGuid());

            order.DomainEvents.Should().BeEmpty();
        }

        [Fact]
        public void TestChangeDescription_WhenEverythingIsOkAndNewValueIsEmpty_ValueMustBeSet()
        {
            const string description = "OrderDescription";
            var order = new OrderBuilder().Build();

            order.ChangeDescription(Description.Create(description), Guid.NewGuid());
            order.ClearEvents();

            order.ChangeDescription(Description.Create(""), Guid.NewGuid());

            var descriptionChangedEvent = order.AssertPublishedDomainEvent<OrderDescriptionChangedEvent>();

            order.DomainEvents.Should().HaveCount(1);
            descriptionChangedEvent.AggregateId.Should().Be(order.Id.Value);
            descriptionChangedEvent.OldValue.Should().Be(description);
            descriptionChangedEvent.NewValue.Should().BeNull();
        }

        [Fact]
        public void TestChangeStatus_WhenEverythingIsOk_ValueMustBeSet()
        {
            var order = new OrderBuilder().Build();
            var orderId = Guid.NewGuid();

            order.ClearEvents();
            order.ChangeStatus(OrderStatus.Delivered, orderId);

            order.UpdaterId.Should().Be(orderId);

            var priceChangedEvent = order.AssertPublishedDomainEvent<OrderStatusChangedEvent>();

            priceChangedEvent.AggregateId.Should().Be(order.Id.Value);
            priceChangedEvent.OldValue.Should().Be(OrderStatus.Received.ToString());
            priceChangedEvent.NewValue.Should().Be(OrderStatus.Delivered.ToString());
        }

        [Fact]
        public void TestChangeStatus_WhenValueIsSame_NothingMustBeHappened()
        {
            var order = new OrderBuilder().Build();

            order.ClearEvents();

            order.ChangeStatus(OrderStatus.Received, Guid.NewGuid());

            order.DomainEvents.Should().BeEmpty();
            order.Status.Should().Be(OrderStatus.Received);
        }

        [Fact]
        public void TestChangeOrderPostType_WhenEverythingIsOkAndIsFragileIsTrue_PropertiesShouldHaveCorrectValues()
        {
            var order = new OrderBuilder().Build();

            order.ClearEvents();

            order.ChangOrderPostType(true, Guid.NewGuid());

            order.PostType.Should().Be(OrderPostType.SpecialPost);

            var orderPostTypeChangedEvent = order.AssertPublishedDomainEvent<OrderPostTypeChangedEvent>();

            order.DomainEvents.Should().HaveCount(1);
            orderPostTypeChangedEvent.AggregateId.Should().Be(order.Id.Value);
            orderPostTypeChangedEvent.OldValue.Should().BeNull();
            orderPostTypeChangedEvent.NewValue.Should().Be(OrderPostType.SpecialPost.ToString());
        }

        [Fact]
        public void TestChangeOrderPostType_WhenEverythingIsOkAndIsFragileIsFalse_PropertiesShouldHaveCorrectValues()
        {
            var order = new OrderBuilder().Build();

            order.ClearEvents();

            order.ChangOrderPostType(false, Guid.NewGuid());

            order.PostType.Should().Be(OrderPostType.OrdinaryPost);

            var orderPostTypeChangedEvent = order.AssertPublishedDomainEvent<OrderPostTypeChangedEvent>();

            order.DomainEvents.Should().HaveCount(1);
            orderPostTypeChangedEvent.AggregateId.Should().Be(order.Id.Value);
            orderPostTypeChangedEvent.OldValue.Should().BeNull();
            orderPostTypeChangedEvent.NewValue.Should().Be(OrderPostType.OrdinaryPost.ToString());
        }

        [Fact]
        public void TestChangeOrderPostType_WhenContainsFragileItemAndOrderPostTypeIsSpecial_NothingMustBeHappened()
        {
            var order = new OrderBuilder().Build();

            order.ChangOrderPostType(true, Guid.NewGuid());
            order.PostType.Should().Be(OrderPostType.SpecialPost);

            order.ClearEvents();

            order.ChangOrderPostType(true, Guid.NewGuid());
            order.DomainEvents.Should().HaveCount(0);
        }

        [Fact]
        public void
            TestChangeOrderPostType_WhenDoNotContainsFragileItemAndOrderPostTypeIsOridnary_NothingMustBeHappened()
        {
            var order = new OrderBuilder().Build();

            order.ChangOrderPostType(false, Guid.NewGuid());
            order.PostType.Should().Be(OrderPostType.OrdinaryPost);

            order.ClearEvents();

            order.ChangOrderPostType(false, Guid.NewGuid());
            order.DomainEvents.Should().HaveCount(0);
        }

        [Fact]
        public void
            TestChangeOrderPostType_WhenDoNotContainFragileItemAndOrderPostTypeIsOrdinary_PropertiesShouldHaveCorrectValues()
        {
            var order = new OrderBuilder().Build();

            order.ChangOrderPostType(false, Guid.NewGuid());

            order.ClearEvents();

            order.ChangOrderPostType(true, Guid.NewGuid());
            order.PostType.Should().Be(OrderPostType.SpecialPost);

            var orderPostTypeChangedEvent = order.AssertPublishedDomainEvent<OrderPostTypeChangedEvent>();

            order.DomainEvents.Should().HaveCount(1);
            orderPostTypeChangedEvent.AggregateId.Should().Be(order.Id.Value);
            orderPostTypeChangedEvent.OldValue.Should().Be(OrderPostType.OrdinaryPost.ToString());
            orderPostTypeChangedEvent.NewValue.Should().Be(OrderPostType.SpecialPost.ToString());
        }

        [Fact]
        public void
            TestChangeOrderPostType_WhenContainFragileItemAndOrderPostTypeIsSpecial_PropertiesShouldHaveCorrectValues()
        {
            var order = new OrderBuilder().Build();

            order.ChangOrderPostType(true, Guid.NewGuid());

            order.ClearEvents();

            order.ChangOrderPostType(false, Guid.NewGuid());
            order.PostType.Should().Be(OrderPostType.OrdinaryPost);

            var orderPostTypeChangedEvent = order.AssertPublishedDomainEvent<OrderPostTypeChangedEvent>();

            order.DomainEvents.Should().HaveCount(1);
            orderPostTypeChangedEvent.AggregateId.Should().Be(order.Id.Value);
            orderPostTypeChangedEvent.OldValue.Should().Be(OrderPostType.SpecialPost.ToString());
            orderPostTypeChangedEvent.NewValue.Should().Be(OrderPostType.OrdinaryPost.ToString());
        }

        [Fact]
        public void TestChangeItems_WhenEverythingIsOk_PropertiesShouldHaveCorrectValues()
        {
            var firstOrderItem = OrderItem.Create(GoodId.Create(Guid.NewGuid()), 2);
            var secondOrderItem = OrderItem.Create(GoodId.Create(Guid.NewGuid()), 1);

            var validator = new Mock<IGoodsTotalPriceValidator>();
            validator.Setup(i => i.IsTotalPriceValid(It.IsAny<IEnumerable<Guid>>())).Returns(Task.FromResult(true));

            var order = new OrderBuilder().Build();

            order.ChangeItems(new[] {firstOrderItem, secondOrderItem}, validator.Object);

            order.DomainEvents.Should().HaveCount(3);

            var firstItem = order.OrderItems.First(i => i.GoodId == firstOrderItem.GoodId);
            firstItem.Count.Should().Be(2);

            var secondItem = order.OrderItems.First(i => i.GoodId == secondOrderItem.GoodId);
            secondItem.Count.Should().Be(1);
        }

        [Fact]
        public void TestChangeItems_WhenOrderItemsUpdated_PropertiesShouldHaveCorrectValues()
        {
            var firstOrderItem = OrderItem.Create(GoodId.Create(Guid.NewGuid()), 2);

            var validator = new Mock<IGoodsTotalPriceValidator>();
            validator.Setup(i => i.IsTotalPriceValid(It.IsAny<IEnumerable<Guid>>())).Returns(Task.FromResult(true));

            var order = new OrderBuilder().Build();

            order.ChangeItems(new[] {firstOrderItem}, validator.Object);

            var secondOrderItem = OrderItem.Create(GoodId.Create(Guid.NewGuid()), 1);

            order.ChangeItems(new[] {secondOrderItem}, validator.Object);

            order.OrderItems.Should().HaveCount(1);
            order.OrderItems.First().GoodId.Should().Be(secondOrderItem.GoodId);
            order.OrderItems.First().Count.Should().Be(1);
        }

        [Fact]
        public void TestDelete_WhenEverythingIsOk_MustBeMarkedAsDeleted()
        {
            var order = new OrderBuilder().Build();

            order.ClearEvents();
            order.Delete();

            var deletedEvent = order.AssertPublishedDomainEvent<OrderDeletedEvent>();

            deletedEvent.AggregateId.Should().Be(order.Id.Value);

            order.CanBeDeleted().Should().BeTrue();
            order.DomainEvents.Should().HaveCount(1);
        }

        [Fact]
        public void TestDelete_WhenAlreadyDeleted_ThrowsException()
        {
            var order = new OrderBuilder().Build();

            order.Delete();

            var action = new Action(() => order.Delete());
            action.Should().Throw<InvalidOperationException>();
        }
    }
}