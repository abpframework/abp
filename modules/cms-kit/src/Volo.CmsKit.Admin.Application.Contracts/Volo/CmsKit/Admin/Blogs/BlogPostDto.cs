using System;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Admin.Blogs
{
    public class BlogPostDto : EntityDto<Guid>
    {
        public Guid BlogId { get; protected set; }

        public string Title { get; protected set; }

        public string UrlSlug { get; protected set; }

        public string CoverImageUrl { get; set; }

        public bool IsPublished { get; protected set; }

        public DateTime? PublishDate { get; protected set; }
    }
}
