using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Project.Application.Properties;
using Project.Domain.Exceptions;

namespace Project.Application.Aggregates.Users.Commands.DeleteUser
{
    public sealed class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly ISystemEntityDetector systemEntityDetector;

        private readonly IUserWriteRepository userWriteRepository;

        public DeleteUserCommandHandler(
            IUserWriteRepository userWriteRepository,
            ISystemEntityDetector systemEntityDetector)
        {
            this.userWriteRepository = userWriteRepository;
            this.systemEntityDetector = systemEntityDetector;
        }

        public async Task<Unit> Handle(
            DeleteUserCommand request,
            CancellationToken cancellationToken)
        {
            if (systemEntityDetector.IsSystemEntity(request.Username))
                throw new DomainException(ApplicationResources.User_UnableToDeleteSystemDefined);

            var user = await userWriteRepository
                .GetByUsername(request.Username, cancellationToken)
                .ConfigureAwait(false);

            if (user is null)
                throw new DomainException(ApplicationResources.User_UnableToFind);

            userWriteRepository.Remove(user);

            return Unit.Value;
        }
    }
}