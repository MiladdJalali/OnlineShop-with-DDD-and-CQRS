using Project.Domain.Aggregates.Users.ValueObjects;
using Project.Domain.ValueObjects;

namespace Project.Domain.Aggregates.Users.Events
{
    public class UserDescriptionChangedEvent : BaseDomainEvent
    {
        public UserDescriptionChangedEvent(UserId userId, Description oldValue, Description newValue)
            : base(userId.Value)
        {
            OldValue = oldValue?.Value;
            NewValue = newValue?.Value;
        }

        public string OldValue { get; }

        public string NewValue { get; }
    }
}