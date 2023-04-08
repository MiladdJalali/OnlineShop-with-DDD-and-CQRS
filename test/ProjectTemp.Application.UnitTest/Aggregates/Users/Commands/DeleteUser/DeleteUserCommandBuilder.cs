using Project.Application.Aggregates.Users.Commands.DeleteUser;

namespace Project.Application.UnitTest.Aggregates.Users.Commands.DeleteUser
{
    public static class DeleteUserCommandBuilder
    {
        public static DeleteUserCommand Build()
        {
            return new DeleteUserCommand
            {
                Username = "UserName"
            };
        }
    }
}