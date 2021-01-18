using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Volo.CmsKit.Blogs
{
    public class Blog : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public string Name { get; set; }

        public Guid? TenantId { get; }
    }
}
