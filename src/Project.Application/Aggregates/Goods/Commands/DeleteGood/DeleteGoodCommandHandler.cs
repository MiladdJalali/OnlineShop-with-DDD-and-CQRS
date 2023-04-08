using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Project.Application.Properties;
using Project.Domain.Exceptions;

namespace Project.Application.Aggregates.Goods.Commands.DeleteGood
{
    public sealed class DeleteGoodCommandHandler : IRequestHandler<DeleteGoodCommand>
    {
        private readonly IGoodWriteRepository goodWriteRepository;
        private readonly ISystemEntityDetector systemEntityDetector;

        public DeleteGoodCommandHandler(
            IGoodWriteRepository goodWriteRepository,
            ISystemEntityDetector systemEntityDetector)
        {
            this.goodWriteRepository = goodWriteRepository;
            this.systemEntityDetector = systemEntityDetector;
        }

        public async Task<Unit> Handle(
            DeleteGoodCommand request,
            CancellationToken cancellationToken)
        {
            if (systemEntityDetector.IsSystemEntity(request.Name))
                throw new DomainException(ApplicationResources.Good_UnableToDeleteSystemDefined);

            var good = await goodWriteRepository
                .GetByName(request.Name, cancellationToken)
                .ConfigureAwait(false);

            if (good is null)
                throw new DomainException(ApplicationResources.Good_UnableToFind);

            goodWriteRepository.Remove(good);

            return Unit.Value;
        }
    }
}