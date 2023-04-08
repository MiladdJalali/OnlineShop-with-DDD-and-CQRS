using Project.Domain.Aggregates.Users.ValueObjects;

namespace Project.Domain.Aggregates.Users.Events
{
    public class UserUsernameChangedEvent : BaseDomainEvent
    {
        public UserUsernameChangedEvent(UserId userId, UserUsername oldValue, UserUsername newValue)
            : base(userId.Value)
        {
            OldValue = oldValue?.Value;
            NewValue = newValue.Value;
        }

        public string OldValue { get; }

        public string NewValue { get; }
    }
}