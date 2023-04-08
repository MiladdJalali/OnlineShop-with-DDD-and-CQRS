using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Project.Application.Properties;
using Project.Application.Services;
using Project.Domain.Aggregates.Users.ValueObjects;
using Project.Domain.Exceptions;
using Project.Domain.ValueObjects;

namespace Project.Application.Aggregates.Users.Commands.UpdateUser
{
    public sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly ISystemEntityDetector systemEntityDetector;

        private readonly IUserDescriptor userDescriptor;

        private readonly IUserWriteRepository userWriteRepository;

        public UpdateUserCommandHandler(
            IUserWriteRepository userWriteRepository,
            ISystemEntityDetector systemEntityDetector,
            IUserDescriptor userDescriptor)
        {
            this.userWriteRepository = userWriteRepository;
            this.systemEntityDetector = systemEntityDetector;
            this.userDescriptor = userDescriptor;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            if (systemEntityDetector.IsSystemEntity(request.CurrentUsername!))
                throw new DomainException(ApplicationResources.User_UnableToUpdateSystemDefined);

            if (systemEntityDetector.IsSystemEntity(request.Username!))
                throw new DomainException(ApplicationResources.User_UsernameCannotStartWithUnderscore);

            var user = await userWriteRepository
                .GetByUsername(request.CurrentUsername!, cancellationToken)
                .ConfigureAwait(false);

            if (user is null)
                throw new DomainException(ApplicationResources.User_UnableToFind);

            var updaterId = userDescriptor.GetId();
            user.ChangeUsername(UserUsername.Create(request.Username), updaterId);
            user.ChangeAddress(UserAddress.Create(request.Address), updaterId);
            user.ChangeDescription(Description.Create(request.Description), updaterId);

            //TODO:changePasswordCommand must be added

            return Unit.Value;
        }
    }
}