using Project.Domain.Aggregates.Users.ValueObjects;

namespace Project.Domain.Aggregates.Users.Events
{
    public class UserDeletedEvent : BaseDomainEvent
    {
        public UserDeletedEvent(UserId userId)
            : base(userId.Value)
        {
        }
    }
}