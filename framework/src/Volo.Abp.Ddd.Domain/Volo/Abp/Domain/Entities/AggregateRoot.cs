using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Auditing;
using Volo.Abp.Data;

namespace Volo.Abp.Domain.Entities
{
    [Serializable]
    public abstract class AggregateRoot : Entity, 
        IAggregateRoot,
        IGeneratesDomainEvents, 
        IHasExtraProperties,
        IHasConcurrencyStamp
    {
        public Dictionary<string, object> ExtraProperties { get; protected set; }

        [DisableAuditing]
        public string ConcurrencyStamp { get; set; }

        private readonly ICollection<object> _localEvents = new Collection<object>();
        private readonly ICollection<object> _distributedEvents = new Collection<object>();

        protected AggregateRoot()
        {
            ExtraProperties = new Dictionary<string, object>();
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
    }

    [Serializable]
    public abstract class AggregateRoot<TKey> : Entity<TKey>, 
        IAggregateRoot<TKey>, 
        IGeneratesDomainEvents, 
        IHasExtraProperties,
        IHasConcurrencyStamp
    {
        public Dictionary<string, object> ExtraProperties { get; protected set; }

        [DisableAuditing]
        public string ConcurrencyStamp { get; set; }

        private readonly ICollection<object> _localEvents = new Collection<object>();
        private readonly ICollection<object> _distributedEvents = new Collection<object>();

        protected AggregateRoot()
        {
            ExtraProperties = new Dictionary<string, object>();
        }

        protected AggregateRoot(TKey id)
            : base(id)
        {
            ExtraProperties = new Dictionary<string, object>();
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
    }
}