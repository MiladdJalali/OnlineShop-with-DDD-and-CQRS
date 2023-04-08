using Project.Domain.Aggregates.Users.ValueObjects;

namespace Project.Domain.Aggregates.Users.Events
{
    public class UserCreatedEvent : BaseDomainEvent
    {
        public UserCreatedEvent(UserId userId)
            : base(userId.Value)
        {
        }
    }
}