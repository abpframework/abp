using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;
using Volo.Abp.Data;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.Domain.Entities
{
    [Serializable]
    public abstract class AggregateRoot : BasicAggregateRoot,
        IHasExtraProperties,
        IHasConcurrencyStamp
    {
        public virtual ExtraPropertyDictionary ExtraProperties { get; protected set; }

        [DisableAuditing]
        public virtual string ConcurrencyStamp { get; set; }

        protected AggregateRoot()
        {
            ConcurrencyStamp = Guid.NewGuid().ToString("N");
            ExtraProperties = new ExtraPropertyDictionary();
            this.SetDefaultsForExtraProperties();
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
    public abstract class AggregateRoot<TKey> : BasicAggregateRoot<TKey>,
        IHasExtraProperties,
        IHasConcurrencyStamp
    {
        public virtual ExtraPropertyDictionary ExtraProperties { get; protected set; }

        [DisableAuditing]
        public virtual string ConcurrencyStamp { get; set; }

        protected AggregateRoot()
        {
            ConcurrencyStamp = Guid.NewGuid().ToString("N");
            ExtraProperties = new ExtraPropertyDictionary();
            this.SetDefaultsForExtraProperties();
        }

        protected AggregateRoot(TKey id)
            : base(id)
        {
            ConcurrencyStamp = Guid.NewGuid().ToString("N");
            ExtraProperties = new ExtraPropertyDictionary();
            this.SetDefaultsForExtraProperties();
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
