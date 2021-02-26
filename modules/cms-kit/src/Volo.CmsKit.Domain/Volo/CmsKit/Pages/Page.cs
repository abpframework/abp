using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Volo.CmsKit.Pages
{
    public class Page : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        [CanBeNull] public virtual Guid? TenantId { get; protected set; }

        [NotNull] public virtual string Title { get; protected set; }

        [NotNull] public virtual string Slug { get; protected set; }

        [CanBeNull] public virtual string Description { get; set; }

        protected Page()
        {
        }

        public Page(Guid id, [NotNull] string title, [NotNull] string slug, [CanBeNull] string description = null,
            Guid? tenantId = null) : base(id)
        {
            Title = Check.NotNullOrEmpty(title, nameof(title), PageConsts.MaxTitleLength);
            Slug = Check.NotNullOrEmpty(slug, nameof(slug), PageConsts.MaxSlugLength);
            Description = Check.Length(description, nameof(description), PageConsts.MaxDescriptionLength);

            TenantId = tenantId;
        }

        public virtual void SetTitle(string title)
        {
            Title = Check.NotNullOrEmpty(title, nameof(title), PageConsts.MaxTitleLength);
        }

        public virtual void SetSlug(string slug)
        {
            Slug = Check.NotNullOrEmpty(slug, nameof(slug), PageConsts.MaxSlugLength);
        }
    }
}