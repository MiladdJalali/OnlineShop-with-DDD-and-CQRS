using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Project.Application.Aggregates.Goods;
using Project.Application.Properties;
using Project.Application.Services;
using Project.Domain.Aggregates.Orders;
using Project.Domain.Aggregates.Orders.Services;
using Project.Domain.Aggregates.Orders.ValueObjects;
using Project.Domain.Exceptions;
using Project.Domain.ValueObjects;

namespace Project.Application.Aggregates.Orders.Commands.CreateOrder
{
    public sealed class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
    {
        private readonly IGoodsTotalPriceValidator goodsTotalPriceValidator;
        private readonly IGoodWriteRepository goodWriteRepository;

        private readonly IOrderTimeValidator orderTimeValidator;

        private readonly IOrderWriteRepository orderWriteRepository;

        private readonly IUserDescriptor userDescriptor;

        public CreateOrderCommandHandler(
            IOrderWriteRepository orderWriteRepository,
            IGoodWriteRepository goodWriteRepository,
            IUserDescriptor userDescriptor,
            IGoodsTotalPriceValidator goodsTotalPriceValidator,
            IOrderTimeValidator orderTimeValidator)
        {
            this.orderWriteRepository = orderWriteRepository;
            this.goodWriteRepository = goodWriteRepository;
            this.userDescriptor = userDescriptor;
            this.goodsTotalPriceValidator = goodsTotalPriceValidator;
            this.orderTimeValidator = orderTimeValidator;
        }

        public async Task<Guid> Handle(
            CreateOrderCommand request,
            CancellationToken cancellationToken)
        {
            var creatorId = userDescriptor.GetId();

            var order = Order.Create(OrderId.Create(Guid.NewGuid()), creatorId, orderTimeValidator);

            var items = new List<OrderItem>();
            foreach (var name in request.GoodsName.Distinct())
            {
                var good = await goodWriteRepository.GetByName(name, cancellationToken).ConfigureAwait(false);
                if (good == null)
                    throw new DomainException(string.Format(ApplicationResources.Order_UnableToFindGood, name));

                items.Add(OrderItem.Create(good.Id, good.IsFragile, request.GoodsName.Select(i => i == name).Count()));
            }

            order.ChangeItems(items.ToArray(), goodsTotalPriceValidator);
            order.ChangeDescription(Description.Create(request.Description), creatorId);

            orderWriteRepository.Add(order);

            return order.Id.Value;
        }
    }
}