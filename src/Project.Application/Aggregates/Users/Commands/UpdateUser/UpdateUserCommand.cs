using MediatR;

namespace Project.Application.Aggregates.Users.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest
    {
        public string CurrentUsername { get; set; }

        public string Username { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }
    }
}