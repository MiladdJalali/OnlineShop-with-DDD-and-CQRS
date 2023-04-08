using System.Threading;
using Moq;
using Project.Application.Aggregates.Goods;
using Project.Application.Aggregates.Goods.Commands.UpdateGood;
using Project.Application.Services;
using Project.Domain.UnitTest.Aggregates.Goods.Builders;

namespace Project.Application.UnitTest.Aggregates.Goods.Commands.UpdateGood
{
    public class UpdateGoodCommandHandlerBuilder
    {
        private readonly IUserDescriptor userDescriptor;

        private IGoodWriteRepository goodWriteRepository;

        private ISystemEntityDetector systemEntityDetector;

        public UpdateGoodCommandHandlerBuilder()
        {
            var goodWriteRepositoryMock = new Mock<IGoodWriteRepository>();
            var good = new GoodBuilder().Build();

            goodWriteRepositoryMock
                .Setup(i => i.GetByName(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(good);

            goodWriteRepository = goodWriteRepositoryMock.Object;
            systemEntityDetector = new Mock<ISystemEntityDetector>().Object;
            userDescriptor = new Mock<IUserDescriptor>().Object;
        }

        public UpdateGoodCommandHandler Build()
        {
            return new UpdateGoodCommandHandler(goodWriteRepository, systemEntityDetector, userDescriptor);
        }

        public UpdateGoodCommandHandlerBuilder WithGoodWriteRepository(IGoodWriteRepository value)
        {
            goodWriteRepository = value;
            return this;
        }

        public UpdateGoodCommandHandlerBuilder WithSystemEntityDetector(ISystemEntityDetector value)
        {
            systemEntityDetector = value;
            return this;
        }
    }
}