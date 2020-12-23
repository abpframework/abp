using JetBrains.Annotations;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Volo.CmsKit.Tags
{
    public class Tag : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public string EntityType { get; set; }
        
        public string Name { get; set; }
        
        public string ColorHex { get; set; }
        
        public Guid? TenantId { get; }
        
        protected Tag()
        {
        }

        public Tag([NotNull] string entityType, [NotNull] string name, [CanBeNull] string colorHex, Guid? tenantId = null)
        {
            EntityType = Check.NotNullOrWhiteSpace(entityType, nameof(entityType), TagConsts.MaxEntityTypeLength);
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), TagConsts.MaxNameLength);
            ColorHex = Check.Length(colorHex, nameof(colorHex), TagConsts.MaxColorHexLength);
            TenantId = tenantId;
        }
    }
}
