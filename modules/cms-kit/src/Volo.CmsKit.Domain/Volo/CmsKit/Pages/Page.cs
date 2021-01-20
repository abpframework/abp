using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Volo.CmsKit.Pages
{
    public class Page : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }

        public virtual string Title { get; set; }

        public virtual string Url { get; set; }
        
        public virtual string Description { get; set; }
        
        protected Page()
        {
            
        }

        public Page(Guid id, [NotNull] string title, [NotNull] string url, [CanBeNull] string description = null, Guid? tenantId = null) : base(id)
        {
            Title = Check.NotNullOrWhiteSpace(title, nameof(title), PageConsts.MaxTitleLength);
            Url = Check.NotNullOrWhiteSpace(url, nameof(url), PageConsts.MaxUrlLength);
            Description = Check.Length(description, nameof(description), PageConsts.MaxDescriptionLength);
            
            TenantId = tenantId;
        }
    }
}