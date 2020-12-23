using System;
using Volo.Abp.Domain.Entities;

namespace Volo.CmsKit.Tags
{
    public class EntityTag : AggregateRoot
    {
        protected EntityTag()
        {
        }

        public EntityTag(string entityType, string entityId, Guid tagId)
        {
            EntityType = entityType;
            EntityId = entityId;
            TagId = tagId;
        }

        public virtual string EntityType { get; protected set; }

        public virtual string EntityId { get; protected set; }

        public virtual Guid TagId { get; set; }

        public override object[] GetKeys()
        {
            return new object[]
            {
                EntityType,
                EntityId,
                TagId
            };
        }
    }
}
