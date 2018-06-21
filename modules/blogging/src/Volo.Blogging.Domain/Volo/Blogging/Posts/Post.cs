using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Volo.Blogging.Posts
{
    public class Post : FullAuditedAggregateRoot<Guid>
    {
        public virtual Guid BlogId { get; protected set; }

        [NotNull]
        public virtual string Title { get; protected set; }

        [CanBeNull]
        public virtual string Content { get; set; }

        protected Post()
        {
            
        }

        public Post(Guid id, Guid blogId, Guid creatorId, [NotNull] string title)
        {
            Id = id;
            CreatorId = creatorId;
            BlogId = blogId;
            Title = Check.NotNullOrWhiteSpace(title, nameof(title));
        }

        public virtual Post SetTitle([NotNull] string title)
        {
            Title = Check.NotNullOrWhiteSpace(title, nameof(title));
            return this;
        }
    }
}
