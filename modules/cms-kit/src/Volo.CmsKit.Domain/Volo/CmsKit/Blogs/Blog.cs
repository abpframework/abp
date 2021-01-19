using JetBrains.Annotations;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Volo.CmsKit.Blogs
{
    public class Blog : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Blog(
            [NotNull] string name,
            [CanBeNull] Guid? tenantId = null)
        {
            SetName(name);
            TenantId = tenantId;
        }

        public string Name { get; protected set; }

        public Guid? TenantId { get; }

        public void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), maxLength: BlogConsts.MaxNameLength);
        }
    }
}
