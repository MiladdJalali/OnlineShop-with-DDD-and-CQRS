using System;
using FluentAssertions;
using Project.Domain.Aggregates.Users.Events;
using Project.Domain.Aggregates.Users.ValueObjects;
using Project.Domain.UnitTest.Aggregates.Users.Builders;
using Project.Domain.UnitTest.Helpers;
using Project.Domain.ValueObjects;
using Xunit;

namespace Project.Domain.UnitTest.Aggregates.Users
{
    public class UserUnitTest
    {
        [Fact]
        public void TestChangeDescription_WhenEverythingIsOk_ValueMustBeSet()
        {
            const string description = "UserDescription";
            var userId = Guid.NewGuid();
            var user = new UserBuilder()
                .WithId(userId)
                .Build();

            user.ClearEvents();
            user.ChangeDescription(Description.Create(description), userId);

            var descriptionChangedEvent = user.AssertPublishedDomainEvent<UserDescriptionChangedEvent>();

            user.UpdaterId.Should().Be(userId);
            user.Description?.Value.Should().Be(description);

            descriptionChangedEvent.AggregateId.Should().Be(userId);
            descriptionChangedEvent.OldValue.Should().BeNull();
            descriptionChangedEvent.NewValue.Should().Be(description);
        }

        [Fact]
        public void TestChangeDescription_WhenValueIsSame_NothingMustBeHappened()
        {
            const string description = "UserDescription";
            var user = new UserBuilder().Build();

            user.ChangeDescription(Description.Create(description), Guid.NewGuid());
            user.ClearEvents();

            user.ChangeDescription(Description.Create(description), Guid.NewGuid());

            user.DomainEvents.Should().BeEmpty();
        }

        [Fact]
        public void TestChangeDescription_WhenEverythingIsOkAndNewValueIsEmpty_ValueMustBeSet()
        {
            const string description = "UserDescription";
            var user = new UserBuilder().Build();

            user.ChangeDescription(Description.Create(description), Guid.NewGuid());
            user.ClearEvents();
            user.ChangeDescription(Description.Create(""), Guid.NewGuid());

            var descriptionChangedEvent = user.AssertPublishedDomainEvent<UserDescriptionChangedEvent>();

            user.DomainEvents.Should().HaveCount(1);
            descriptionChangedEvent.AggregateId.Should().Be(user.Id.Value);
            descriptionChangedEvent.OldValue.Should().Be(description);
            descriptionChangedEvent.NewValue.Should().BeNull();
        }

        [Fact]
        public void TestChangePassword_WhenEverythingIsOk_ValueMustBeSet()
        {
            const string oldPassword = "OldUserPassword";
            const string newPassword = "NewUserPassword";
            var userId = Guid.NewGuid();
            var user = new UserBuilder().WithId(userId).WithPassword(oldPassword).Build();

            user.ClearEvents();
            user.ChangePassword(UserPassword.Create(newPassword), userId);
            user.UpdaterId.Should().Be(userId);

            var passwordChangedEvent = user.AssertPublishedDomainEvent<UserPasswordChangedEvent>();

            passwordChangedEvent.AggregateId.Should().Be(userId);
            user.Password.Value.Should().Be(newPassword);
        }

        [Fact]
        public void TestChangeAddress_WhenEverythingIsOk_ValueMustBeSet()
        {
            const string oldAddress = "OldUserAddress";
            const string newAddress = "NewUserAddress";
            var userId = Guid.NewGuid();
            var user = new UserBuilder().WithId(userId).WithAddress(oldAddress).Build();

            user.ClearEvents();
            user.ChangeAddress(UserAddress.Create(newAddress), userId);
            user.UpdaterId.Should().Be(userId);

            var addressChangedEvent = user.AssertPublishedDomainEvent<UserAddressChangedEvent>();

            addressChangedEvent.AggregateId.Should().Be(userId);
            addressChangedEvent.OldValue.Should().Be(oldAddress);
            addressChangedEvent.NewValue.Should().Be(newAddress);
            user.Address.Value.Should().Be(newAddress);
        }

        [Fact]
        public void TestChangeAddress_WhenValueIsSame_NothingMustBeHappened()
        {
            const string address = "UserAddress";
            var user = new UserBuilder()
                .WithAddress(address)
                .Build();

            user.ClearEvents();
            user.ChangeAddress(UserAddress.Create(address), Guid.NewGuid());

            user.DomainEvents.Should().BeEmpty();
        }

        [Fact]
        public void TestChangeUsername_WhenEverythingIsOk_ValueMustBeSet()
        {
            const string oldUsername = "OldUserUsername";
            const string newUsername = "NewUserUsername";
            var userId = Guid.NewGuid();
            var user = new UserBuilder().WithId(userId).WithUsername(oldUsername).Build();

            user.ClearEvents();
            user.ChangeUsername(UserUsername.Create(newUsername), userId);
            user.UpdaterId.Should().Be(userId);

            var usernameChangedEvent = user.AssertPublishedDomainEvent<UserUsernameChangedEvent>();

            usernameChangedEvent.AggregateId.Should().Be(userId);
            usernameChangedEvent.OldValue.Should().Be(oldUsername);
            usernameChangedEvent.NewValue.Should().Be(newUsername);
            user.Username.Value.Should().Be(newUsername);
        }

        [Fact]
        public void TestChangeUsername_WhenValueIsSame_NothingMustBeHappened()
        {
            const string username = "UserUsername";
            var user = new UserBuilder().WithUsername(username).Build();

            user.ClearEvents();
            user.ChangeUsername(UserUsername.Create(username), Guid.NewGuid());

            user.DomainEvents.Should().BeEmpty();
        }

        [Fact]
        public void TestCreate_WhenEverythingIsOk_PropertiesShouldHaveCorrectValues()
        {
            const string username = "UserUsername";
            const string password = "UserPassword";
            const string address = "Address";

            var userId = Guid.NewGuid();

            var user = new UserBuilder()
                .WithId(userId)
                .WithUsername(username)
                .WithPassword(password)
                .WithAddress(address)
                .WithCreatorId(userId)
                .Build();

            var createdEvent = user.AssertPublishedDomainEvent<UserCreatedEvent>();
            createdEvent.AggregateId.Should().Be(userId);

            var usernameChangedEvent = user.AssertPublishedDomainEvent<UserUsernameChangedEvent>();
            usernameChangedEvent.AggregateId.Should().Be(userId);
            usernameChangedEvent.OldValue.Should().BeNull();
            usernameChangedEvent.NewValue.Should().Be(username);

            var passwordChangedEvent = user.AssertPublishedDomainEvent<UserPasswordChangedEvent>();
            passwordChangedEvent.AggregateId.Should().Be(userId);

            var addressChangedEvent = user.AssertPublishedDomainEvent<UserAddressChangedEvent>();
            addressChangedEvent.AggregateId.Should().Be(userId);
            addressChangedEvent.OldValue.Should().BeNull();
            addressChangedEvent.NewValue.Should().Be(address);

            user.Id.Value.Should().Be(userId);
            user.Username.Value.Should().Be(username);
            user.Password.Value.Should().Be(password);
            user.Address.Value.Should().Be(address);
            user.Description.Should().BeNull();
            user.CreatorId.Should().Be(userId);
            user.UpdaterId.Should().Be(userId);
        }

        [Fact]
        public void TestDelete_WhenEverythingIsOk_MustBeMarkedAsDeleted()
        {
            var user = new UserBuilder().Build();

            user.ClearEvents();
            user.Delete();

            var deletedEvent = user.AssertPublishedDomainEvent<UserDeletedEvent>();

            deletedEvent.AggregateId.Should().Be(user.Id.Value);

            user.CanBeDeleted().Should().BeTrue();
            user.DomainEvents.Should().HaveCount(1);
        }

        [Fact]
        public void TestDelete_WhenAlreadyDeleted_ThrowsException()
        {
            var user = new UserBuilder().Build();

            user.Delete();

            var action = new Action(() => user.Delete());
            action.Should().Throw<InvalidOperationException>();
        }
    }
}