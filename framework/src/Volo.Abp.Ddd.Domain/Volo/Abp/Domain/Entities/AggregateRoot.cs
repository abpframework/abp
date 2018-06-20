using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Volo.Abp.Domain.Entities
{
    [Serializable]
    public abstract class AggregateRoot : Entity, IAggregateRoot, IGeneratesDomainEvents
    {
        private readonly ICollection<object> _domainEvents = new Collection<object>();

        protected virtual void AddDomainEvent(object eventData)
        {
            _domainEvents.Add(eventData);
        }

        public virtual IEnumerable<object> GetDomainEvents()
        {
            return _domainEvents;
        }

        public virtual void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }

    [Serializable]
    public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot<TKey>, IGeneratesDomainEvents
    {
        private readonly ICollection<object> _domainEvents = new Collection<object>();

        protected AggregateRoot()
        {
            
        }

        protected AggregateRoot(TKey id)
            : base(id)
        {

        }

        protected virtual void AddDomainEvent(object eventData)
        {
            _domainEvents.Add(eventData);
        }

        public virtual IEnumerable<object> GetDomainEvents()
        {
            return _domainEvents;
        }

        public virtual void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}