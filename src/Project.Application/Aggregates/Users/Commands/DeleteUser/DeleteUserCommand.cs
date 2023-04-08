using MediatR;

namespace Project.Application.Aggregates.Users.Commands.DeleteUser
{
    public class DeleteUserCommand : IRequest
    {
        public string Username { get; set; }
    }
}