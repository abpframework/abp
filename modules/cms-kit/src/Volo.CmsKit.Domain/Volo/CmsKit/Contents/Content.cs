using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Volo.CmsKit.Contents
{
    public class Content : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; }
        
        public virtual string EntityType { get; set; }
        
        public virtual string EntityId { get; set; }
        
        public virtual string Value { get; set; }

        protected Content()
        {
            
        }
        
        public Content(Guid id, [NotNull] string entityType, [NotNull] string entityId, [NotNull] string value, Guid? tenantId = null) : base(id)
        {
            EntityType = Check.NotNullOrWhiteSpace(entityType, nameof(entityType), ContentConsts.MaxEntityTypeLength);
            EntityId = Check.NotNullOrWhiteSpace(entityId, nameof(entityId), ContentConsts.MaxEntityIdLength);
            Value = Check.NotNullOrWhiteSpace(value, nameof(value), ContentConsts.MaxValueLength);
            
            TenantId = tenantId;
        }
    }
}
