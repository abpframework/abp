using System;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Admin.Blogs
{
    public class BlogPostDto : EntityDto<Guid>
    {
        public Guid BlogId { get; set; }

        public string Title { get; set; }

        public string UrlSlug { get; set; }
        public string ShortDescription { get; protected set; }
    }
}
