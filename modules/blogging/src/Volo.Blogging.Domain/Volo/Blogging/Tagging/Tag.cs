using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Volo.Blogging.Tagging
{
    public class Tag : FullAuditedAggregateRoot<Guid>
    {
        public virtual Guid BlogId { get; protected set; }

        public virtual string Name { get; protected set; }

        public virtual string Description { get; protected set; }

        public virtual int UsageCount { get; protected internal set; }

        protected Tag()
        {

        }

        public Tag(Guid id, Guid blogId, [NotNull] string name, int usageCount = 0, string description = null)
        {
            Id = id;
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            BlogId = blogId;
            Description = description;
            UsageCount = usageCount;
        }

        public virtual void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
        }

        public virtual void IncreaseUsageCount(int number = 1)
        {
            UsageCount += number;
        }

        public virtual void DecreaseUsageCount(int number = 1)
        {
            if (UsageCount <= 0)
            {
                return;
            }

            if (UsageCount - number <= 0)
            {
                UsageCount = 0;
                return;
            }

            UsageCount -= number;
        }

        public virtual void SetDescription(string description)
        {
            Description = description;
        }
    }
}
