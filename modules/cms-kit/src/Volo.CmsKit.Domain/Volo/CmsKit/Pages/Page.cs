using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Volo.CmsKit.Pages
{
    public class Page : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; }

        public virtual string Title { get; set; }

        public virtual string Description { get; set; }
        
        public virtual string Url { get; set; }

        protected Page()
        {
            
        }

        public Page(Guid id, [NotNull] string title, [CanBeNull] string description, [CanBeNull] string url = null, Guid? tenantId = null) : base(id)
        {
            Title = Check.NotNullOrWhiteSpace(title, nameof(title), PageConsts.MaxTitleLength);
            Description = Check.Length(description, nameof(description), PageConsts.MaxDescriptionLength);
            SetUrl(url);
            
            TenantId = tenantId;
        }

        public virtual void SetUrl(string url)
        {
            Url = Check.Length(url, nameof(url), PageConsts.MaxUrlLength);   
        }
    }
}