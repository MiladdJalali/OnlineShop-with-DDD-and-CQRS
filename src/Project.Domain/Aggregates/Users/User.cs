using System;
using Project.Domain.Aggregates.Users.Events;
using Project.Domain.Aggregates.Users.ValueObjects;
using Project.Domain.ValueObjects;

namespace Project.Domain.Aggregates.Users
{
    public class User : Entity, IAggregateRoot
    {
        private User()
        {
        }

        public UserId Id { get; private init; }

        public UserUsername Username { get; private set; }

        public UserPassword Password { get; private set; }

        public UserAddress Address { get; private set; }

        public Description Description { get; private set; }

        public static User Create(
            UserId userId,
            UserUsername username,
            UserPassword password,
            UserAddress address,
            Guid creatorId)
        {
            var user = new User {Id = userId};

            user.AddEvent(new UserCreatedEvent(user.Id));
            user.TrackCreate(creatorId);

            user.ChangeUsername(username, creatorId);
            user.ChangePassword(password, creatorId);
            user.ChangeAddress(address, creatorId);

            return user;
        }

        public void ChangeUsername(UserUsername username, Guid updaterId)
        {
            if (Username == username)
                return;

            AddEvent(new UserUsernameChangedEvent(Id, Username, username));
            TrackUpdate(updaterId);

            Username = username;
        }

        public void ChangePassword(UserPassword password, Guid updaterId)
        {
            AddEvent(new UserPasswordChangedEvent(Id));
            TrackUpdate(updaterId);

            Password = password;
        }

        public void ChangeAddress(UserAddress address, Guid updaterId)
        {
            if (Address == address)
                return;

            AddEvent(new UserAddressChangedEvent(Id, Address, address));
            TrackUpdate(updaterId);

            Address = address;
        }

        public void ChangeDescription(Description description, Guid updaterId)
        {
            if (Description?.Value == description?.Value)
                return;

            AddEvent(new UserDescriptionChangedEvent(Id, Description, description));
            TrackUpdate(updaterId);

            Description = description;
        }

        public void Delete()
        {
            if (CanBeDeleted())
                throw new InvalidOperationException();

            AddEvent(new UserDeletedEvent(Id));
            MarkAsDeleted();
        }
    }
}