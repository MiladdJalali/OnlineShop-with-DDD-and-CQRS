using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Project.Application.Properties;
using Project.Domain.Exceptions;

namespace Project.Application.Aggregates.Orders.Commands.DeleteOrder
{
    public sealed class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
    {
        private readonly IOrderWriteRepository orderWriteRepository;

        public DeleteOrderCommandHandler(
            IOrderWriteRepository orderWriteRepository)
        {
            this.orderWriteRepository = orderWriteRepository;
        }

        public async Task<Unit> Handle(
            DeleteOrderCommand request,
            CancellationToken cancellationToken)
        {
            var order = await orderWriteRepository
                .GetById(request.Id, cancellationToken)
                .ConfigureAwait(false);

            if (order is null)
                throw new DomainException(ApplicationResources.Order_UnableToFind);

            orderWriteRepository.Remove(order);

            return Unit.Value;
        }
    }
}