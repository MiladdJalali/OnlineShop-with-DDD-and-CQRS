using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Project.Application.Aggregates.Goods;
using Project.Application.Properties;
using Project.Application.Services;
using Project.Domain.Aggregates.Orders.Enums;
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

            if (order.Status != OrderStatus.Received)
                throw new DomainException(ApplicationResources.Order_PackedOrDeliverdOrderCanNotbeUpdated);

            var updaterId = userDescriptor.GetId();
            var items = new List<OrderItem>();
            var containsFragileItems = false;

            foreach (var orderGood in request.Goods)
            {
                var good = await goodWriteRepository.GetByName(orderGood.Name, cancellationToken).ConfigureAwait(false);
                if (good == null)
                    throw new DomainException(string.Format(ApplicationResources.Order_UnableToFindGood, orderGood.Name));

                items.Add(OrderItem.Create(good.Id, orderGood.Count));

                if (good.IsFragile.Value)
                    containsFragileItems = true;
            }

            order.ChangeItems(items.ToArray(), validator);
            order.ChangPostType(containsFragileItems, updaterId);
            order.ChangeDescription(Description.Create(request.Description), updaterId);

            return Unit.Value;
        }
    }
}