using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Project.Application.Aggregates.Users;
using Project.Application.Properties;
using Project.Domain.Exceptions;
using Xunit;

namespace Project.Application.UnitTest.Aggregates.Users.Commands.DeleteUser
{
    public class DeleteUserCommandHandlerUnitTest
    {
        [Fact]
        public void TestHandle_WhenUserIsSystemEntity_ThrowsException()
        {
            var systemEntityDetectorMock = new Mock<ISystemEntityDetector>();
            var command = DeleteUserCommandBuilder.Build();
            var commandHandler = new DeleteUserCommandHandlerBuilder()
                .WithSystemEntityDetector(systemEntityDetectorMock.Object)
                .Build();
            var func = new Func<Task>(async () => await commandHandler
                .Handle(command, CancellationToken.None));

            systemEntityDetectorMock.Setup(i => i.IsSystemEntity(command.Username)).Returns(true);

            func.Should().ThrowAsync<DomainException>()
                .WithMessage(ApplicationResources.User_UnableToDeleteSystemDefined);
        }

        [Fact]
        public void TestHandle_WhenUserDoesNotExit_ThrowsException()
        {
            var systemEntityDetectorMock = new Mock<ISystemEntityDetector>();
            var userWriteRepositoryMock = new Mock<IUserWriteRepository>();
            var command = DeleteUserCommandBuilder.Build();
            var commandHandler = new DeleteUserCommandHandlerBuilder()
                .WithSystemEntityDetector(systemEntityDetectorMock.Object)
                .WithUserWriteRepository(userWriteRepositoryMock.Object)
                .Build();
            var func = new Func<Task>(async () => await commandHandler
                .Handle(command, CancellationToken.None));

            systemEntityDetectorMock.Setup(i => i.IsSystemEntity(command.Username)).Returns(false);

            userWriteRepositoryMock
                .Setup(i => i.GetByUsername(command.Username, CancellationToken.None))
                .ReturnsAsync(() => null);

            func.Should().ThrowAsync<DomainException>().WithMessage(ApplicationResources.User_UnableToFind);
        }
    }
}