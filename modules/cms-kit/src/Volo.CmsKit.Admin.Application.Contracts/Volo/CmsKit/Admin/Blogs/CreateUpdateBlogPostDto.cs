using System;

namespace Volo.CmsKit.Admin.Blogs
{
    public class CreateUpdateBlogPostDto
    {
        public Guid BlogId { get; protected set; }

        public string Title { get; protected set; }

        public string UrlSlug { get; protected set; }

        public string CoverImageUrl { get; set; }
    }
}
