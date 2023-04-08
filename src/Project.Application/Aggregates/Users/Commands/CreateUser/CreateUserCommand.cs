using MediatR;

namespace Project.Application.Aggregates.Users.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<string>
    {
        public string Username { get; set; }

        public string Address { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string Description { get; set; }
    }
}