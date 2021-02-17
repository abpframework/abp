using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;
using Volo.CmsKit.Blogs;

namespace Volo.CmsKit.Admin.Blogs
{
    public class UpdateBlogPostDto
    {
        [Required]
        [DynamicMaxLength(typeof(BlogPostConsts), nameof(BlogPostConsts.MaxTitleLength))]
        public string Title { get; set; }

        [Required]
        [DynamicStringLength(
            typeof(BlogPostConsts),
            nameof(BlogPostConsts.MaxSlugLength),
            nameof(BlogPostConsts.MinSlugLength))]
        public string Slug { get; set; }

        [DynamicMaxLength(typeof(BlogPostConsts), nameof(BlogPostConsts.MaxShortDescriptionLength))]
        public string ShortDescription { get; set; }

    }
}
