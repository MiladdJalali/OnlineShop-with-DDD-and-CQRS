using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Project.Application.Properties;
using Project.Application.Services;
using Project.Domain.Aggregates.Orders.Enums;
using Project.Domain.Exceptions;

namespace Project.Application.Aggregates.Orders.Commands.ChangeOrderStatus
{
    public sealed class ChangeOrderStatusCommandHandler : IRequestHandler<ChangeOrderStatusCommand>
    {
        private readonly IOrderWriteRepository orderWriteRepository;

        private readonly IUserDescriptor userDescriptor;

        public ChangeOrderStatusCommandHandler(
            IOrderWriteRepository orderWriteRepository,
            IUserDescriptor userDescriptor)
        {
            this.orderWriteRepository = orderWriteRepository;
            this.userDescriptor = userDescriptor;
        }

        public async Task<Unit> Handle(
            ChangeOrderStatusCommand request,
            CancellationToken cancellationToken)
        {
            var order = await orderWriteRepository
                .GetById(request.OrderId, cancellationToken)
                .ConfigureAwait(false);

            if (order is null)
                throw new DomainException(ApplicationResources.Order_UnableToFind);

            if (!Enum.TryParse<OrderStatus>(request.Status, true, out var status))
                throw new DomainException(ApplicationResources.Order_InvalidStatusProvided);

            order.ChangeStatus(status, userDescriptor.GetId());

            return Unit.Value;
        }
    }
}