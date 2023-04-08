using System.Threading;
using Moq;
using Project.Application.Aggregates.Goods;
using Project.Application.Aggregates.Goods.Commands.DeleteGood;
using Project.Domain.UnitTest.Aggregates.Goods.Builders;

namespace Project.Application.UnitTest.Aggregates.Goods.Commands.DeleteGood
{
    public class DeleteGoodCommandHandlerBuilder
    {
        private IGoodWriteRepository goodWriteRepository;

        private ISystemEntityDetector systemEntityDetector;

        public DeleteGoodCommandHandlerBuilder()
        {
            var goodWriteRepositoryMock = new Mock<IGoodWriteRepository>();
            var good = new GoodBuilder().Build();

            goodWriteRepositoryMock
                .Setup(i => i.GetByName(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(good);

            goodWriteRepository = goodWriteRepositoryMock.Object;
            systemEntityDetector = new Mock<ISystemEntityDetector>().Object;
        }

        public DeleteGoodCommandHandler Build()
        {
            return new DeleteGoodCommandHandler(goodWriteRepository, systemEntityDetector);
        }

        public DeleteGoodCommandHandlerBuilder WithGoodWriteRepository(IGoodWriteRepository value)
        {
            goodWriteRepository = value;
            return this;
        }

        public DeleteGoodCommandHandlerBuilder WithSystemEntityDetector(ISystemEntityDetector value)
        {
            systemEntityDetector = value;
            return this;
        }
    }
}