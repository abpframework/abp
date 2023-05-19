using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;
using Volo.CmsKit.Tags;

namespace Volo.CmsKit.Admin.Tags;

[Serializable]
public class TagCreateDto
{
    [Required]
    [DynamicMaxLength(typeof(TagConsts), nameof(TagConsts.MaxEntityTypeLength))]
    public string EntityType { get; set; }

    [Required]
    [DynamicMaxLength(typeof(TagConsts), nameof(TagConsts.MaxNameLength))]
    public string Name { get; set; }
}
