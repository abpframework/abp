using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Volo.CmsKit.Pages
{
    public class Page : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }

        public virtual string Title { get; protected set; }

        public virtual string Slug { get; protected set; }

        public virtual string Content { get; protected set; }
        
        protected Page()
        {
        }

        internal Page(Guid id, [NotNull] string title, [NotNull] string slug, string content = null, Guid? tenantId = null) : base(id)
        {
            TenantId = tenantId;
            
            SetTitle(title);
            SetSlug(slug);
            SetContent(content);
        }

        public virtual void SetTitle(string title)
        {
            Title = Check.NotNullOrEmpty(title, nameof(title), PageConsts.MaxTitleLength);
        }

        internal virtual void SetSlug(string slug)
        {
            Slug = Check.NotNullOrEmpty(slug, nameof(slug), PageConsts.MaxSlugLength);
        }

        public virtual void SetContent(string content)
        {
            Content = Check.Length(content, nameof(content), PageConsts.MaxContentLength);
        }
    }
}