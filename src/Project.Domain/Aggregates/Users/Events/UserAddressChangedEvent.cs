using Project.Domain.Aggregates.Users.ValueObjects;

namespace Project.Domain.Aggregates.Users.Events
{
    public class UserAddressChangedEvent : BaseDomainEvent
    {
        public UserAddressChangedEvent(UserId userId, UserAddress oldValue, UserAddress newValue)
            : base(userId.Value)
        {
            OldValue = oldValue?.Value;
            NewValue = newValue.Value;
        }

        public string OldValue { get; }

        public string NewValue { get; }
    }
}