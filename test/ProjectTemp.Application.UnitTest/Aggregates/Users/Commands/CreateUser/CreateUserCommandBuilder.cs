using Project.Application.Aggregates.Users.Commands.CreateUser;

namespace Project.Application.UnitTest.Aggregates.Users.Commands.CreateUser
{
    public static class CreateUserCommandBuilder
    {
        public static CreateUserCommand Build()
        {
            return new CreateUserCommand
            {
                Username = "UserUsername",
                Address = "Address",
                Password = "UserPassword",
                ConfirmPassword = "UserPassword",
                Description = "UserDescription"
            };
        }
    }
}