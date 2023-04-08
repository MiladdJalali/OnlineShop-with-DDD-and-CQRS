using Project.Domain.Aggregates.Users.ValueObjects;

namespace Project.Domain.Aggregates.Users.Events
{
    public class UserPasswordChangedEvent : BaseDomainEvent
    {
        public UserPasswordChangedEvent(UserId userId)
            : base(userId.Value)
        {
        }
    }
}