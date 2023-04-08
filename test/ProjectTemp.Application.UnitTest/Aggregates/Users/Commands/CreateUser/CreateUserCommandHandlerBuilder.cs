using Moq;
using Project.Application.Aggregates.Users;
using Project.Application.Aggregates.Users.Commands.CreateUser;
using Project.Application.Services;

namespace Project.Application.UnitTest.Aggregates.Users.Commands.CreateUser
{
    public class CreateUserCommandHandlerBuilder
    {
        private readonly IUserDescriptor userDescriptor;
        private ISystemEntityDetector systemEntityDetector;

        public CreateUserCommandHandlerBuilder()
        {
            systemEntityDetector = new Mock<ISystemEntityDetector>().Object;
            userDescriptor = new Mock<IUserDescriptor>().Object;
        }

        public CreateUserCommandHandler Build()
        {
            return new CreateUserCommandHandler(
                new Mock<IUserWriteRepository>().Object,
                systemEntityDetector,
                userDescriptor);
        }

        public CreateUserCommandHandlerBuilder WithSystemEntityDetector(ISystemEntityDetector value)
        {
            systemEntityDetector = value;
            return this;
        }
    }
}