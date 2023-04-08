using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Project.Application.Properties;
using Project.Application.Services;
using Project.Domain.Aggregates.Goods.ValueObjects;
using Project.Domain.Exceptions;
using Project.Domain.ValueObjects;

namespace Project.Application.Aggregates.Goods.Commands.UpdateGood
{
    public sealed class UpdateGoodCommandHandler : IRequestHandler<UpdateGoodCommand>
    {
        private readonly IGoodWriteRepository goodWriteRepository;

        private readonly ISystemEntityDetector systemEntityDetector;

        private readonly IUserDescriptor userDescriptor;

        public UpdateGoodCommandHandler(
            IGoodWriteRepository goodWriteRepository,
            ISystemEntityDetector systemEntityDetector,
            IUserDescriptor userDescriptor)
        {
            this.goodWriteRepository = goodWriteRepository;
            this.systemEntityDetector = systemEntityDetector;
            this.userDescriptor = userDescriptor;
        }

        public async Task<Unit> Handle(UpdateGoodCommand request, CancellationToken cancellationToken)
        {
            if (systemEntityDetector.IsSystemEntity(request.CurrentName!))
                throw new DomainException(ApplicationResources.Good_UnableToUpdateSystemDefined);

            if (systemEntityDetector.IsSystemEntity(request.Name))
                throw new DomainException(ApplicationResources.Good_NameCannotStartWithUnderscore);

            var good = await goodWriteRepository
                .GetByName(request.CurrentName!, cancellationToken)
                .ConfigureAwait(false);

            if (good is null)
                throw new DomainException(ApplicationResources.Good_UnableToFind);

            var updaterId = userDescriptor.GetId();

            good.ChangeName(GoodName.Create(request.Name), updaterId);
            good.ChangePrice(GoodPrice.Create(request.Price), updaterId);
            good.ChangeDiscount(GoodDiscount.Create(request.Discount), updaterId);
            good.ChangeIsFragile(GoodIsFragile.Create(request.IsFragile), updaterId);
            good.ChangeDescription(Description.Create(request.Description), updaterId);

            return Unit.Value;
        }
    }
}