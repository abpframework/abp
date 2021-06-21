using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Volo.Abp.Domain.Entities
{
    [Serializable]
    public abstract class BasicAggregateRoot : Entity,
        IAggregateRoot,
        IGeneratesDomainEvents
    {
        private readonly ICollection<object> _distributedEvents = new Collection<object>();
        private readonly ICollection<object> _localEvents = new Collection<object>();

        public virtual IEnumerable<object> GetLocalEvents()
        {
            return _localEvents;
        }

        public virtual IEnumerable<object> GetDistributedEvents()
        {
            return _distributedEvents;
        }

        public virtual void ClearLocalEvents()
        {
            _localEvents.Clear();
        }

        public virtual void ClearDistributedEvents()
        {
            _distributedEvents.Clear();
        }

        protected virtual void AddLocalEvent(object eventData)
        {
            _localEvents.Add(eventData);
        }

        protected virtual void AddDistributedEvent(object eventData)
        {
            _distributedEvents.Add(eventData);
        }
    }

    [Serializable]
    public abstract class BasicAggregateRoot<TKey> : Entity<TKey>,
        IAggregateRoot<TKey>,
        IGeneratesDomainEvents
    {
        private readonly ICollection<object> _distributedEvents = new Collection<object>();
        private readonly ICollection<object> _localEvents = new Collection<object>();

        protected BasicAggregateRoot()
        {

        }

        protected BasicAggregateRoot(TKey id)
            : base(id)
        {

        }

        public virtual IEnumerable<object> GetLocalEvents()
        {
            return _localEvents;
        }

        public virtual IEnumerable<object> GetDistributedEvents()
        {
            return _distributedEvents;
        }

        public virtual void ClearLocalEvents()
        {
            _localEvents.Clear();
        }

        public virtual void ClearDistributedEvents()
        {
            _distributedEvents.Clear();
        }

        protected virtual void AddLocalEvent(object eventData)
        {
            _localEvents.Add(eventData);
        }

        protected virtual void AddDistributedEvent(object eventData)
        {
            _distributedEvents.Add(eventData);
        }
    }
}
