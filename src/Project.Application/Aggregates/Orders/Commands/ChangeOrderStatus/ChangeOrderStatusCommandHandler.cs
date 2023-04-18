using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Project.Application.Aggregates.Users;
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

        private readonly IUserWriteRepository userWriteRepository;

        public ChangeOrderStatusCommandHandler(
            IOrderWriteRepository orderWriteRepository,
            IUserWriteRepository userWriteRepository,
            IUserDescriptor userDescriptor)
        {
            this.orderWriteRepository = orderWriteRepository;
            this.userWriteRepository = userWriteRepository;
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

            var user = await userWriteRepository
                .GetById(order.CreatorId, cancellationToken)
                .ConfigureAwait(false);
            if (user is null)
                throw new DomainException(ApplicationResources.Order_UserDoesNotExist);

            var updaterId = userDescriptor.GetId();

            switch (order.Status)
            {
                case OrderStatus.Received:
                    order.ChangeStatus(OrderStatus.Packed, updaterId);
                    order.ChangeAddress(user.Address, updaterId);
                    break;
                case OrderStatus.Packed:
                    order.ChangeStatus(OrderStatus.Delivered, updaterId);
                    break;
                case OrderStatus.Delivered:
                default:
                    throw new DomainException(ApplicationResources.Order_DeliveredOrderStatusCanNotBeChanged);
            }

            return Unit.Value;
        }
    }
}