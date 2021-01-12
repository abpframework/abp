using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Volo.CmsKit.Domain.Volo.CmsKit.Blogs
{
    public class BlogPost : FullAuditedEntity<Guid>
    {
        public Guid BlogId { get; set; }
        public string Title { get; set; }
        public string CoverImageUrl { get; set; }
        public bool IsPublished { get; set; }
    }
}
