using System;

namespace Project.Application.Services
{
    public interface IUserDescriptor
    {
        Guid GetId();

        string GetClient();

        string GetClientAddress();
    }
}