using JetBrains.Annotations;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Volo.CmsKit.Blogs;

public class Blog : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    [NotNull]
    public virtual string Name { get; protected set; }

    [NotNull]
    public virtual string Slug { get; protected set; }

    public virtual Guid? TenantId { get; protected set; }

    protected internal Blog(
        Guid id,
        [NotNull] string name,
        [NotNull] string slug,
        [CanBeNull] Guid? tenantId = null
        ) : base(id)
    {
        SetName(name);
        SetSlug(slug);
        TenantId = tenantId;
    }

    public virtual void SetName(string name)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), BlogConsts.MaxNameLength);
    }

    public virtual void SetSlug(string slug)
    {
        Check.NotNullOrWhiteSpace(slug, nameof(slug), BlogConsts.MaxNameLength);

        Slug = SlugNormalizer.Normalize(slug);
    }
}
