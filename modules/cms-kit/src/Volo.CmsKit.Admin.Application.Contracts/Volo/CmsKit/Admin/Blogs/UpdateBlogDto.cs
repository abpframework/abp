using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Validation;
using Volo.CmsKit.Blogs;

namespace Volo.CmsKit.Admin.Blogs;

public class UpdateBlogDto : IHasConcurrencyStamp
{
    [Required]
    [DynamicMaxLength(typeof(BlogConsts), nameof(BlogConsts.MaxNameLength))]
    public string Name { get; set; }

    [Required]
    [DynamicMaxLength(typeof(BlogConsts), nameof(BlogConsts.MaxSlugLength))]
    public string Slug { get; set; }

    public string ConcurrencyStamp { get; set; }
}
