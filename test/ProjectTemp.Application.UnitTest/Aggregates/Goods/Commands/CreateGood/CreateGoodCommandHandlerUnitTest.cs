using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Project.Application.Properties;
using Project.Domain.Exceptions;
using Xunit;

namespace Project.Application.UnitTest.Aggregates.Goods.Commands.CreateGood
{
    public class CreateGoodCommandHandlerUnitTest
    {
        [Fact]
        public async Task TestHandle_WhenEverythingIsOk_GoodNameMustBeReturned()
        {
            var command = CreateGoodCommandBuilder.Build();
            var commandHandler = new CreateGoodCommandHandlerBuilder().Build();

            var goodName = await commandHandler.Handle(command, CancellationToken.None);

            goodName.Should().Be(command.Name);
        }

        [Fact]
        public void TestHandle_WhenNameIsSystemEntity_ThrowsException()
        {
            var systemEntityDetectorMock = new Mock<ISystemEntityDetector>();
            var command = CreateGoodCommandBuilder.Build();
            var commandHandler = new CreateGoodCommandHandlerBuilder()
                .WithSystemEntityDetector(systemEntityDetectorMock.Object)
                .Build();
            var func = new Func<Task>(async () => await commandHandler
                .Handle(command, CancellationToken.None));

            systemEntityDetectorMock.Setup(i => i.IsSystemEntity(command.Name)).Returns(true);

            func.Should().ThrowAsync<DomainException>()
                .WithMessage(ApplicationResources.Good_NameCannotStartWithUnderscore);
        }
    }
}