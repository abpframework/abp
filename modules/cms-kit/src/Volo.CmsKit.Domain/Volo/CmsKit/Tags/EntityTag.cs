using System;
using JetBrains.Annotations;
using Volo.Abp;
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

        internal EntityTag(Guid tagId, [NotNull] string entityId, Guid? tenantId = null)
        {
            TagId = tagId;
            EntityId = Check.NotNullOrEmpty(entityId,nameof(entityId));
            TenantId = tenantId;
        }

        public override object[] GetKeys()
        {
            return new object[] { TagId, EntityId };
        }
    }
}
