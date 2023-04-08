using Moq;
using Project.Application.Aggregates.Goods;
using Project.Application.Aggregates.Goods.Commands.CreateGood;
using Project.Application.Services;

namespace Project.Application.UnitTest.Aggregates.Goods.Commands.CreateGood
{
    public class CreateGoodCommandHandlerBuilder
    {
        private readonly IUserDescriptor userDescriptor;
        private ISystemEntityDetector systemEntityDetector;

        public CreateGoodCommandHandlerBuilder()
        {
            systemEntityDetector = new Mock<ISystemEntityDetector>().Object;
            userDescriptor = new Mock<IUserDescriptor>().Object;
        }

        public CreateGoodCommandHandler Build()
        {
            return new CreateGoodCommandHandler(
                new Mock<IGoodWriteRepository>().Object,
                systemEntityDetector,
                userDescriptor);
        }

        public CreateGoodCommandHandlerBuilder WithSystemEntityDetector(ISystemEntityDetector value)
        {
            systemEntityDetector = value;
            return this;
        }
    }
}