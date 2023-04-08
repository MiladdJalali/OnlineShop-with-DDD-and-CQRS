using System;
using System.Threading.Tasks;
using Project.Application.Aggregates.Users;

namespace Project.RestApi.Services
{
    public class UserService : IUserService
    {
        private readonly IPasswordHashProvider passwordHashProvider;
        private readonly IUserWriteRepository userRepository;

        public UserService(
            IUserWriteRepository userRepository,
            IPasswordHashProvider passwordHashProvider)
        {
            this.userRepository = userRepository;
            this.passwordHashProvider = passwordHashProvider;
        }

        public async Task<Guid?> ValidateCredentials(string username, string password)
        {
            var user = await userRepository.GetByUsername(username).ConfigureAwait(false);

            if (user == null || user.Password.Value != passwordHashProvider.Hash(password))
                return null;

            return user.Id.Value;
        }
    }
}