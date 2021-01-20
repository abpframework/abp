using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Volo.CmsKit.Tags
{
    public class EntityTag : Entity, IMultiTenant
    {
        public virtual Guid TagId { get; set; }

        public virtual string EntityId { get; set; }
        
        public virtual Guid? TenantId { get; set; }
        
        protected EntityTag()
        {
        }

        internal EntityTag(Guid tagId, string entityId, Guid? tenantId = null)
        {
            TagId = tagId;
            EntityId = entityId;
            TenantId = tenantId;
        }

        public override object[] GetKeys()
        {
            return new object[] { TagId, EntityId };
        }
    }
}
