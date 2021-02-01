using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;
using Volo.CmsKit.Blogs;

namespace Volo.CmsKit.Admin.Blogs
{
    public class CreateUpdateBlogPostDto
    {
        [Required]
        public Guid BlogId { get; set; }

        [Required]
        [DynamicMaxLength(typeof(BlogPostConsts), nameof(BlogPostConsts.MaxTitleLength))]
        public string Title { get; set; }

        [Required]
        [DynamicStringLength(
            typeof(BlogPostConsts),
            nameof(BlogPostConsts.MaxUrlSlugLength),
            nameof(BlogPostConsts.MinUrlSlugLength))]
        public string UrlSlug { get; set; }

        [DynamicMaxLength(typeof(BlogPostConsts), nameof(BlogPostConsts.MaxShortDescriptionLength))]
        public string ShortDescription { get; set; }
    }
}
