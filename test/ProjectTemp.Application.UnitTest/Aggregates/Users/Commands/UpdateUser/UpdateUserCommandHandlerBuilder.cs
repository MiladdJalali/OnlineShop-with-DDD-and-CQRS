using System.Threading;
using Moq;
using Project.Application.Aggregates.Users;
using Project.Application.Aggregates.Users.Commands.UpdateUser;
using Project.Application.Services;
using Project.Domain.UnitTest.Aggregates.Users.Builders;

namespace Project.Application.UnitTest.Aggregates.Users.Commands.UpdateUser
{
    public class UpdateUserCommandHandlerBuilder
    {
        private readonly IUserDescriptor userDescriptor;
        private ISystemEntityDetector systemEntityDetector;

        private IUserWriteRepository userWriteRepository;

        public UpdateUserCommandHandlerBuilder()
        {
            var userWriteRepositoryMock = new Mock<IUserWriteRepository>();
            var user = new UserBuilder().Build();

            userWriteRepositoryMock
                .Setup(i => i.GetByUsername(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(user);

            userWriteRepository = userWriteRepositoryMock.Object;
            systemEntityDetector = new Mock<ISystemEntityDetector>().Object;
            userDescriptor = new Mock<IUserDescriptor>().Object;
        }

        public UpdateUserCommandHandler Build()
        {
            return new UpdateUserCommandHandler(userWriteRepository, systemEntityDetector, userDescriptor);
        }

        public UpdateUserCommandHandlerBuilder WithUserWriteRepository(IUserWriteRepository value)
        {
            userWriteRepository = value;
            return this;
        }

        public UpdateUserCommandHandlerBuilder WithSystemEntityDetector(ISystemEntityDetector value)
        {
            systemEntityDetector = value;
            return this;
        }
    }
}