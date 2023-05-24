using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;
using Volo.CmsKit.Blogs;

namespace Volo.CmsKit.Admin.Blogs;

[Serializable]
public class UpdateBlogDto : ExtensibleObject, IHasConcurrencyStamp
{
    [Required]
    [DynamicMaxLength(typeof(BlogConsts), nameof(BlogConsts.MaxNameLength))]
    public string Name { get; set; }

    [Required]
    [DynamicMaxLength(typeof(BlogConsts), nameof(BlogConsts.MaxSlugLength))]
    public string Slug { get; set; }

    public string ConcurrencyStamp { get; set; }
}
