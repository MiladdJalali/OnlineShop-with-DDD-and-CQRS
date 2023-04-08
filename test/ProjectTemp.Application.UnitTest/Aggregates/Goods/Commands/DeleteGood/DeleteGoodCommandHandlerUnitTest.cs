using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Project.Application.Aggregates.Goods;
using Project.Application.Properties;
using Project.Domain.Exceptions;
using Xunit;

namespace Project.Application.UnitTest.Aggregates.Goods.Commands.DeleteGood
{
    public class DeleteGoodCommandHandlerUnitTest
    {
        [Fact]
        public void TestHandle_WhenGoodIsSystemEntity_ThrowsException()
        {
            var systemEntityDetectorMock = new Mock<ISystemEntityDetector>();
            var command = DeleteGoodCommandBuilder.Build();
            var commandHandler = new DeleteGoodCommandHandlerBuilder()
                .WithSystemEntityDetector(systemEntityDetectorMock.Object)
                .Build();
            var func = new Func<Task>(async () => await commandHandler
                .Handle(command, CancellationToken.None));

            systemEntityDetectorMock.Setup(i => i.IsSystemEntity(command.Name)).Returns(true);

            func.Should().ThrowAsync<DomainException>()
                .WithMessage(ApplicationResources.Good_UnableToDeleteSystemDefined);
        }

        [Fact]
        public void TestHandle_WhenGoodDoesNotExit_ThrowsException()
        {
            var systemEntityDetectorMock = new Mock<ISystemEntityDetector>();
            var goodWriteRepositoryMock = new Mock<IGoodWriteRepository>();
            var command = DeleteGoodCommandBuilder.Build();
            var commandHandler = new DeleteGoodCommandHandlerBuilder()
                .WithSystemEntityDetector(systemEntityDetectorMock.Object)
                .WithGoodWriteRepository(goodWriteRepositoryMock.Object)
                .Build();
            var func = new Func<Task>(async () => await commandHandler
                .Handle(command, CancellationToken.None));

            systemEntityDetectorMock.Setup(i => i.IsSystemEntity(command.Name)).Returns(false);

            goodWriteRepositoryMock
                .Setup(i => i.GetByName(command.Name, CancellationToken.None))
                .ReturnsAsync(() => null);

            func.Should().ThrowAsync<DomainException>().WithMessage(ApplicationResources.Good_UnableToFind);
        }
    }
}