using System;
using System.Collections.ObjectModel;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Volo.Blogging.Posts
{
    public class Post : FullAuditedAggregateRoot<Guid>
    {
        public virtual Guid BlogId { get; protected set; }

        [NotNull]
        public virtual string Url { get; protected set; }

        [NotNull]
        public virtual string Title { get; protected set; }

        [CanBeNull]
        public virtual string Content { get; set; }

        public virtual Collection<PostTag> Tags { get; protected set; }

        protected Post()
        {
            
        }

        public Post(Guid id, Guid blogId, Guid creatorId, [NotNull] string title, [NotNull] string url)
        {
            Id = id;
            CreatorId = creatorId;
            BlogId = blogId;
            Title = Check.NotNullOrWhiteSpace(title, nameof(title));
            Url = Check.NotNullOrWhiteSpace(url, nameof(url));

            Tags = new Collection<PostTag>();
        }

        public virtual Post SetTitle([NotNull] string title)
        {
            Title = Check.NotNullOrWhiteSpace(title, nameof(title));
            return this;
        }

        public virtual Post SetUrl([NotNull] string url)
        {
            Url = Check.NotNullOrWhiteSpace(url, nameof(url));
            return this;
        }
    }
}
