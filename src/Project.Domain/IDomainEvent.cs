using System;
using System.Collections.Generic;
using MediatR;

namespace Project.Domain
{
    public interface IDomainEvent : INotification
    {
        Guid AggregateId { get; }

        DateTimeOffset EventTime { get; }

        Dictionary<string, object> Flatten();
    }
}