using JetBrains.Annotations;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using Volo.CmsKit.Blogs.Extensions;

namespace Volo.CmsKit.Blogs
{
    public class Blog : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Blog(
            Guid id,
            [NotNull] string name,
            [NotNull] string slug,
            [CanBeNull] Guid? tenantId = null) : base(id)
        {
            SetName(name);
            SetSlug(slug);
            TenantId = tenantId;
        }

        [NotNull] 
        public virtual string Name { get; protected set; }

        [NotNull] 
        public virtual string Slug { get; protected set; }

        public virtual Guid? TenantId { get; protected set; }

        public virtual void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), maxLength: BlogConsts.MaxNameLength);
        }

        public virtual void SetSlug(string slug)
        {
            Check.NotNullOrWhiteSpace(slug, nameof(slug), maxLength: BlogConsts.MaxNameLength);

            Slug = slug.NormalizeSlug();
        }
    }
}
