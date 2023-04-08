using System;
using System.Collections.Generic;
using Project.Domain.Exceptions;

namespace Project.Domain
{
    public abstract class Entity
    {
        private readonly List<IDomainEvent> domainEvents;

        private bool canBeDeleted;

        protected Entity()
        {
            domainEvents = new List<IDomainEvent>();
        }

        public Guid CreatorId { get; private set; }

        public DateTimeOffset Created { get; private set; }

        public Guid? UpdaterId { get; private set; }

        public DateTimeOffset? Updated { get; private set; }

        public IEnumerable<IDomainEvent> DomainEvents => domainEvents.AsReadOnly();

        protected void TrackCreate(Guid creatorId)
        {
            CreatorId = creatorId;
            Created = DateTime.UtcNow;
        }

        protected void TrackUpdate(Guid updaterId)
        {
            UpdaterId = updaterId;
            Updated = DateTime.UtcNow;
        }

        public bool CanBeDeleted()
        {
            return canBeDeleted;
        }

        public void ClearEvents()
        {
            domainEvents.Clear();
        }

        protected void MarkAsDeleted()
        {
            canBeDeleted = true;
        }

        protected void AddEvent(IDomainEvent domainEvent)
        {
            domainEvents.Add(domainEvent);
        }

        protected static void CheckRule(IBusinessRule rule)
        {
            if (!rule.IsBroken())
                return;

            throw new DomainException(rule.Message, rule.Details);
        }
    }
}