using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Validation;
using Volo.CmsKit.Tags;

namespace Volo.CmsKit.Admin.Tags;

[Serializable]
public class TagUpdateDto : IHasConcurrencyStamp
{
    [Required]
    [DynamicMaxLength(typeof(TagConsts), nameof(TagConsts.MaxNameLength))]
    public string Name { get; set; }

    public string ConcurrencyStamp { get; set; }
}
