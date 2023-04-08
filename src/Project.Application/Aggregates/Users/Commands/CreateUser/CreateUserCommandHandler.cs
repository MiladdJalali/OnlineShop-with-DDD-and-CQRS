using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Project.Application.Properties;
using Project.Application.Services;
using Project.Domain.Aggregates.Users;
using Project.Domain.Aggregates.Users.ValueObjects;
using Project.Domain.Exceptions;
using Project.Domain.ValueObjects;

namespace Project.Application.Aggregates.Users.Commands.CreateUser
{
    public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, string>
    {
        private readonly ISystemEntityDetector systemEntityDetector;

        private readonly IUserDescriptor userDescriptor;

        private readonly IUserWriteRepository userWriteRepository;

        public CreateUserCommandHandler(
            IUserWriteRepository userWriteRepository,
            ISystemEntityDetector systemEntityDetector,
            IUserDescriptor userDescriptor)
        {
            this.userWriteRepository = userWriteRepository;
            this.systemEntityDetector = systemEntityDetector;
            this.userDescriptor = userDescriptor;
        }

        public Task<string> Handle(
            CreateUserCommand request,
            CancellationToken cancellationToken)
        {
            if (systemEntityDetector.IsSystemEntity(request.Username!))
                throw new DomainException(ApplicationResources.User_UsernameCannotStartWithUnderscore);

            if (request.Password != request.ConfirmPassword)
                throw new DomainException(ApplicationResources.User_PasswordAndConfirmPasswordDoesNotMatch);

            var creatorId = userDescriptor.GetId();
            var user = User.Create(
                UserId.Create(Guid.NewGuid()),
                UserUsername.Create(request.Username),
                UserPassword.Create(request.Password.GetHash()),
                UserAddress.Create(request.Address),
                creatorId);

            user.ChangeDescription(Description.Create(request.Description), creatorId);

            userWriteRepository.Add(user);

            return Task.FromResult(request.Username!);
        }
    }
}