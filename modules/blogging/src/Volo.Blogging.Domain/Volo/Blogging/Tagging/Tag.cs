using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Volo.Blogging.Tagging
{
    public class Tag : FullAuditedAggregateRoot<Guid>
    {
        public virtual string Name { get; protected set; }

        public virtual string Description { get; protected set; }

        public virtual int UsageCount { get; protected internal set; }

        protected Tag()
        {

        }

        public Tag([NotNull] string name, string description = null)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            Description = description;
        }

        public virtual void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
        }

        public virtual void SetDescription(string description)
        {
            Description = description;
        }
    }
}
