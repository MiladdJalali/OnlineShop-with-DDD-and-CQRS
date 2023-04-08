using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Project.Application.Aggregates.Goods;
using Project.Application.Properties;
using Project.Domain.Exceptions;
using Xunit;

namespace Project.Application.UnitTest.Aggregates.Goods.Commands.UpdateGood
{
    public class UpdateGoodCommandHandlerUnitTest
    {
        [Fact]
        public void TestHandle_WhenGoodIsSystemEntity_ThrowsException()
        {
            var systemEntityDetectorMock = new Mock<ISystemEntityDetector>();
            var command = UpdateGoodCommandBuilder.Build();
            var commandHandler = new UpdateGoodCommandHandlerBuilder()
                .WithSystemEntityDetector(systemEntityDetectorMock.Object)
                .Build();
            var func = new Func<Task>(async () => await commandHandler
                .Handle(command, CancellationToken.None));

            systemEntityDetectorMock.Setup(i => i.IsSystemEntity(command.CurrentName)).Returns(true);

            func.Should().ThrowAsync<DomainException>()
                .WithMessage(ApplicationResources.Good_UnableToUpdateSystemDefined);
        }

        [Fact]
        public void TestHandle_WhenNewGoodNameIsSystemEntity_ThrowsException()
        {
            var systemEntityDetectorMock = new Mock<ISystemEntityDetector>();
            var command = UpdateGoodCommandBuilder.Build();
            var commandHandler = new UpdateGoodCommandHandlerBuilder()
                .WithSystemEntityDetector(systemEntityDetectorMock.Object)
                .Build();
            var func = new Func<Task>(async () => await commandHandler
                .Handle(command, CancellationToken.None));

            systemEntityDetectorMock.Setup(i => i.IsSystemEntity(command.CurrentName)).Returns(false);

            systemEntityDetectorMock.Setup(i => i.IsSystemEntity(command.Name)).Returns(true);

            func.Should().ThrowAsync<DomainException>()
                .WithMessage(ApplicationResources.Good_NameCannotStartWithUnderscore);
        }

        [Fact]
        public void TestHandle_WhenGoodDoesNotExit_ThrowsException()
        {
            var systemEntityDetectorMock = new Mock<ISystemEntityDetector>();
            var goodWriteRepositoryMock = new Mock<IGoodWriteRepository>();
            var command = UpdateGoodCommandBuilder.Build();
            var commandHandler = new UpdateGoodCommandHandlerBuilder()
                .WithSystemEntityDetector(systemEntityDetectorMock.Object)
                .WithGoodWriteRepository(goodWriteRepositoryMock.Object)
                .Build();
            var func = new Func<Task>(async () => await commandHandler
                .Handle(command, CancellationToken.None));

            systemEntityDetectorMock.Setup(i => i.IsSystemEntity(command.CurrentName)).Returns(false);

            systemEntityDetectorMock.Setup(i => i.IsSystemEntity(command.Name)).Returns(false);

            goodWriteRepositoryMock
                .Setup(i => i.GetByName(command.CurrentName, CancellationToken.None))
                .ReturnsAsync(() => null);

            func.Should().ThrowAsync<DomainException>()
                .WithMessage(ApplicationResources.Good_UnableToFind);
        }
    }
}