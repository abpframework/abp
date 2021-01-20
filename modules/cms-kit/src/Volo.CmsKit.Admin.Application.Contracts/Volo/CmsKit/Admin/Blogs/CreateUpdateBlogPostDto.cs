using System;

namespace Volo.CmsKit.Admin.Blogs
{
    public class CreateUpdateBlogPostDto
    {
        public Guid BlogId { get; set; }

        public string Title { get; set; }

        public string UrlSlug { get; set; }

        public string ShortDescription { get; set; }
    }
}
