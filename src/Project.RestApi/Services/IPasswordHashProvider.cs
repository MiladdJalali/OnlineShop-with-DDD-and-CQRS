namespace Project.RestApi.Services
{
    public interface IPasswordHashProvider
    {
        string Hash(string password);
    }
}