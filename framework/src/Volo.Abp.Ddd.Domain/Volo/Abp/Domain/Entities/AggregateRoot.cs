using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Volo.Abp.Domain.Entities
{
    [Serializable]
    public abstract class AggregateRoot : Entity, IAggregateRoot, IGeneratesDomainEvents
    {
        private readonly ICollection<object> _localEvents = new Collection<object>();
        private readonly ICollection<object> _distributedEvents = new Collection<object>();

        protected virtual void AddLocalEvent(object eventData)
        {
            _localEvents.Add(eventData);
        }

        protected virtual void AddDistributedEvent(object eventData)
        {
            _distributedEvents.Add(eventData);
        }

        public virtual IEnumerable<object> GetLocalEvents()
        {
            return _localEvents;
        }

        public IEnumerable<object> GetDistributedEvents()
        {
            return _distributedEvents;
        }

        public virtual void ClearLocalEvents()
        {
            _localEvents.Clear();
        }

        public void ClearDistributedEvents()
        {
            _distributedEvents.Clear();
        }
    }

    [Serializable]
    public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot<TKey>, IGeneratesDomainEvents
    {
        private readonly ICollection<object> _localEvents = new Collection<object>();
        private readonly ICollection<object> _distributedEvents = new Collection<object>();

        protected AggregateRoot()
        {
            
        }

        protected AggregateRoot(TKey id)
            : base(id)
        {

        }

        protected virtual void AddLocalEvent(object eventData)
        {
            _localEvents.Add(eventData);
        }

        protected virtual void AddDistributedEvent(object eventData)
        {
            _distributedEvents.Add(eventData);
        }

        public virtual IEnumerable<object> GetLocalEvents()
        {
            return _localEvents;
        }

        public IEnumerable<object> GetDistributedEvents()
        {
            return _distributedEvents;
        }

        public virtual void ClearLocalEvents()
        {
            _localEvents.Clear();
        }

        public void ClearDistributedEvents()
        {
            _distributedEvents.Clear();
        }
    }
}