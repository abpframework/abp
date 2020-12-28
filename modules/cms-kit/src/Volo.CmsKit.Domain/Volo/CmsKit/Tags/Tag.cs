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

        public string Name { get; protected set; }

        public Guid? TenantId { get; }

        protected Tag()
        {
        }

        public Tag(
            [NotNull] string entityType,
            [NotNull] string name,
            Guid? tenantId = null)
        {
            EntityType = Check.NotNullOrWhiteSpace(entityType, nameof(entityType), TagConsts.MaxEntityTypeLength);
            SetName(name);
            TenantId = tenantId;
        }

        public void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), TagConsts.MaxNameLength);
        }
    }
}
