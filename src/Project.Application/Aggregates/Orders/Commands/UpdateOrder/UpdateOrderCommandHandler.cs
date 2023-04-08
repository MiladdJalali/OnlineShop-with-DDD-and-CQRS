using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Project.Application.Aggregates.Goods;
using Project.Application.Properties;
using Project.Application.Services;
using Project.Domain.Aggregates.Orders.Services;
using Project.Domain.Aggregates.Orders.ValueObjects;
using Project.Domain.Exceptions;
using Project.Domain.ValueObjects;

namespace Project.Application.Aggregates.Orders.Commands.UpdateOrder
{
    public sealed class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
    {
        private readonly IGoodWriteRepository goodWriteRepository;

        private readonly IOrderWriteRepository orderWriteRepository;

        private readonly IUserDescriptor userDescriptor;

        private readonly IGoodsTotalPriceValidator validator;

        public UpdateOrderCommandHandler(
            IGoodWriteRepository goodWriteRepository,
            IOrderWriteRepository orderWriteRepository,
            IUserDescriptor userDescriptor,
            IGoodsTotalPriceValidator validator)
        {
            this.goodWriteRepository = goodWriteRepository;
            this.orderWriteRepository = orderWriteRepository;
            this.userDescriptor = userDescriptor;
            this.validator = validator;
        }

        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await orderWriteRepository
                .GetById(request.OrderId, cancellationToken)
                .ConfigureAwait(false);

            if (order is null)
                throw new DomainException(ApplicationResources.Order_UnableToFind);

            var updaterId = userDescriptor.GetId();
            var items = new List<OrderItem>();
            foreach (var name in request.GoodsName.Distinct())
            {
                var good = await goodWriteRepository.GetByName(name, cancellationToken).ConfigureAwait(false);
                if (good == null)
                    throw new DomainException(string.Format(ApplicationResources.Order_UnableToFindGood, name));

                items.Add(OrderItem.Create(good.Id, good.IsFragile, request.GoodsName.Select(i => i == name).Count()));
            }

            order.ChangeItems(items.ToArray(), validator);

            order.ChangeDescription(Description.Create(request.Description), updaterId);

            return Unit.Value;
        }
    }
}