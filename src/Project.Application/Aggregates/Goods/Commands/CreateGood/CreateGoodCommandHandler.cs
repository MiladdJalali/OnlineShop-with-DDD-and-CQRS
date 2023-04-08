using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Project.Application.Properties;
using Project.Application.Services;
using Project.Domain.Aggregates.Goods;
using Project.Domain.Aggregates.Goods.ValueObjects;
using Project.Domain.Exceptions;
using Project.Domain.ValueObjects;

namespace Project.Application.Aggregates.Goods.Commands.CreateGood
{
    public sealed class CreateGoodCommandHandler : IRequestHandler<CreateGoodCommand, string>
    {
        private readonly IGoodWriteRepository goodWriteRepository;

        private readonly ISystemEntityDetector systemEntityDetector;

        private readonly IUserDescriptor userDescriptor;

        public CreateGoodCommandHandler(
            IGoodWriteRepository goodWriteRepository,
            ISystemEntityDetector systemEntityDetector,
            IUserDescriptor userDescriptor)
        {
            this.goodWriteRepository = goodWriteRepository;
            this.systemEntityDetector = systemEntityDetector;
            this.userDescriptor = userDescriptor;
        }

        public Task<string> Handle(
            CreateGoodCommand request,
            CancellationToken cancellationToken)
        {
            if (systemEntityDetector.IsSystemEntity(request.Name))
                throw new DomainException(ApplicationResources.Good_NameCannotStartWithUnderscore);

            var creatorId = userDescriptor.GetId();

            var good = Good.Create(
                GoodId.Create(Guid.NewGuid()),
                GoodName.Create(request.Name),
                GoodPrice.Create(request.Price),
                GoodDiscount.Create(request.Discount),
                GoodIsFragile.Create(request.IsFragile),
                creatorId);

            good.ChangeDescription(Description.Create(request.Description), creatorId);

            goodWriteRepository.Add(good);

            return Task.FromResult(request.Name);
        }
    }
}