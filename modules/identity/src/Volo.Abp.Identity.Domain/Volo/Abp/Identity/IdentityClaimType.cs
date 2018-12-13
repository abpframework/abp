using System;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Identity
{
    public class IdentityClaimType : AggregateRoot<Guid>
    {
        public virtual string Name { get; protected set; }

        public virtual bool Required { get; protected set; }

        public virtual bool IsStatic { get; protected set; }

        public virtual string Regex { get; protected set; }

        public virtual string RegexDescription { get; protected set; }

        public virtual string Description { get; protected set; }

        public virtual IdentityClaimValueType ValueType { get; protected set; }

        protected IdentityClaimType()
        {

        }

        public IdentityClaimType(Guid id, [NotNull] string name, bool required, bool isStatic, [CanBeNull]string regex, [CanBeNull]string regexDescription, [CanBeNull] string description, IdentityClaimValueType valueType  = IdentityClaimValueType.String)
        {
            Check.NotNull(name, nameof(name));

            Id = id;
            Name = name;
            Required = required;
            IsStatic = isStatic;
            Regex = regex;
            RegexDescription = regexDescription;
            Description = description;
            ValueType = valueType;
        }
    }
}
