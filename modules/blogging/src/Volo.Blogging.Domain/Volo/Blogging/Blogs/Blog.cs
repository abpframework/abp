using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Volo.Blogging.Blogs
{
    public class Blog : FullAuditedAggregateRoot<Guid>
    {
        [NotNull]
        public virtual string Name { get; protected set; }

        [NotNull]
        public virtual string ShortName { get; protected set; }

        [CanBeNull]
        public virtual string Description { get; set; }

        protected Blog()
        {
            
        }

        public Blog(Guid id, [NotNull] string name, [NotNull] string shortName)
        {
            Id = id;
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            ShortName = Check.NotNullOrWhiteSpace(shortName, nameof(shortName));
        }

        public virtual Blog SetName([NotNull] string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            return this;
        }

        public virtual Blog SetShortName(string shortName)
        {
            ShortName = Check.NotNullOrWhiteSpace(shortName, nameof(shortName));
            return this;
        }
    }
}
