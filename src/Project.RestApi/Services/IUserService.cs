using System;
using System.Threading.Tasks;

namespace Project.RestApi.Services
{
    public interface IUserService
    {
        Task<Guid?> ValidateCredentials(string username, string password);
    }
}