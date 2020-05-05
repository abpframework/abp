using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;
using Volo.Abp.Data;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.Domain.Entities
{
    [Serializable]
    public abstract class AggregateRoot : Entity, 
        IAggregateRoot,
        IGeneratesDomainEvents, 
        IHasExtraProperties,
        IHasConcurrencyStamp
    {
        public virtual Dictionary<string, object> ExtraProperties { get; protected set; }

        [DisableAuditing]
        public virtual string ConcurrencyStamp { get; set; }

        private readonly ICollection<object> _localEvents = new Collection<object>();
        private readonly ICollection<object> _distributedEvents = new Collection<object>();

        protected AggregateRoot()
        {
            ConcurrencyStamp = Guid.NewGuid().ToString("N");
            ExtraProperties = new Dictionary<string, object>();
            this.SetDefaultsForExtraProperties();
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

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return ExtensibleObjectValidator.GetValidationErrors(
                this,
                validationContext
            );
        }
    }

    [Serializable]
    public abstract class AggregateRoot<TKey> : Entity<TKey>, 
        IAggregateRoot<TKey>, 
        IGeneratesDomainEvents, 
        IHasExtraProperties,
        IHasConcurrencyStamp
    {
        public virtual Dictionary<string, object> ExtraProperties { get; protected set; }

        [DisableAuditing]
        public virtual string ConcurrencyStamp { get; set; }

        private readonly ICollection<object> _localEvents = new Collection<object>();
        private readonly ICollection<object> _distributedEvents = new Collection<object>();

        protected AggregateRoot()
        {
            ConcurrencyStamp = Guid.NewGuid().ToString("N");
            ExtraProperties = new Dictionary<string, object>();
            this.SetDefaultsForExtraProperties();
        }

        protected AggregateRoot(TKey id)
            : base(id)
        {
            ConcurrencyStamp = Guid.NewGuid().ToString("N");
            ExtraProperties = new Dictionary<string, object>();
            this.SetDefaultsForExtraProperties();
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

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return ExtensibleObjectValidator.GetValidationErrors(
                this,
                validationContext
            );
        }
    }
}