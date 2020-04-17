using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Volo.Blogging.Posts
{
    public class PostTag : CreationAuditedEntity
    {
        public virtual Guid PostId { get; protected set; }

        public virtual Guid TagId { get; protected set; }

        protected PostTag()
        {

        }

        public PostTag(Guid postId, Guid tagId)
        {
            PostId = postId;
            TagId = tagId;
        }

        public override object[] GetKeys()
        {
            return new object[] { PostId, TagId };
        }
    }
}
