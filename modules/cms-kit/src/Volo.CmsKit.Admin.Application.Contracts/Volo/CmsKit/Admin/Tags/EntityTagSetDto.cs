using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.CmsKit.Localization;
using Volo.CmsKit.Tags;

namespace Volo.CmsKit.Admin.Tags;

[Serializable]
public class EntityTagSetDto : IValidatableObject
{
    public string EntityId { get; set; }

    public string EntityType { get; set; }

    [Required]
    public List<string> Tags { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var l = validationContext.GetRequiredService<IStringLocalizer<CmsKitResource>>();

        foreach (var tag in Tags)
        {
            if (tag.Length > TagConsts.MaxNameLength)
            {
                yield return new ValidationResult(
                    l[
                        "MaxTagLengthExceptionMessage",
                        nameof(tag),
                        TagConsts.MaxNameLength,
                        typeof(EntityTagSetDto).FullName
                    ],
                    new[] { nameof(Tags) }
                );
            }
        }
    }
}
