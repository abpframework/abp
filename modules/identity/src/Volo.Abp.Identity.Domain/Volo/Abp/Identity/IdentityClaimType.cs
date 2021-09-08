using JetBrains.Annotations;
using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Identity
{
    public class IdentityClaimType : AggregateRoot<Guid>
    {
        public virtual string Name { get; protected set; }

        public virtual bool Required { get; set; }

        public virtual bool IsStatic { get; protected set; }

        public virtual string Regex { get; set; }

        public virtual string RegexDescription { get; set; }

        public virtual string Description { get; set; }

        public virtual IdentityClaimValueType ValueType { get; set; }

        protected IdentityClaimType()
        {

        }

        public IdentityClaimType(
            Guid id,
            [NotNull] string name,
            bool required = false,
            bool isStatic = false,
            [CanBeNull] string regex = null,
            [CanBeNull] string regexDescription = null,
            [CanBeNull] string description = null,
            IdentityClaimValueType valueType = IdentityClaimValueType.String)
        {
            Id = id;
            SetName(name);
            Required = required;
            IsStatic = isStatic;
            Regex = regex;
            RegexDescription = regexDescription;
            Description = description;
            ValueType = valueType;
        }

        public void SetName([NotNull] string name)
        {
            Name = Check.NotNull(name, nameof(name));
        }
    }
}
