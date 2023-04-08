using System.Security.Cryptography;
using System.Text;

namespace Project.RestApi.Services
{
    public class PasswordHashProvider : IPasswordHashProvider
    {
        public string Hash(string password)
        {
            using var sha256 = SHA256.Create();

            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            var builder = new StringBuilder();

            foreach (var @byte in bytes)
                builder.Append(@byte.ToString("x2"));

            return builder.ToString();
        }
    }
}