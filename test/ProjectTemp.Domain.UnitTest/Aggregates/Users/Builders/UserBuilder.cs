using System;
using Project.Domain.Aggregates.Users;
using Project.Domain.Aggregates.Users.ValueObjects;

namespace Project.Domain.UnitTest.Aggregates.Users.Builders
{
    public class UserBuilder
    {
        private string address;

        private Guid creatorId;
        private Guid id;

        private string password;

        private string username;

        public UserBuilder()
        {
            id = Guid.NewGuid();
            username = "Username";
            address = "Address";
            password = "Password";
        }

        public User Build()
        {
            return User.Create(
                UserId.Create(id),
                UserUsername.Create(username),
                UserPassword.Create(password),
                UserAddress.Create(address),
                creatorId);
        }

        public UserBuilder WithId(Guid value)
        {
            id = value;
            return this;
        }

        public UserBuilder WithUsername(string value)
        {
            username = value;
            return this;
        }

        public UserBuilder WithPassword(string value)
        {
            password = value;
            return this;
        }

        public UserBuilder WithAddress(string value)
        {
            address = value;
            return this;
        }

        public UserBuilder WithCreatorId(Guid value)
        {
            creatorId = value;
            return this;
        }
    }
}