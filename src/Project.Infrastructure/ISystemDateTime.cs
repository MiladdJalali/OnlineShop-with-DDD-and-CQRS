using System;

namespace Project.Infrastructure
{
    public interface ISystemDateTime
    {
        DateTimeOffset UtcNow { get; }
    }
}