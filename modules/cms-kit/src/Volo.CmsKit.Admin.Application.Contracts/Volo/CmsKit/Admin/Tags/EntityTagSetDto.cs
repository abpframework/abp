using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.CmsKit.Tags;

namespace Volo.CmsKit.Admin.Tags;

[Serializable]
public class EntityTagSetDto : IValidatableObject
{
    public string EntityId { get; set; }
    public string EntityType { get; set; }
    public List<string> Tags { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var tag in Tags)
        {
            if (tag.Length > TagConsts.MaxNameLength)
            {
                yield return new ValidationResult(
                    $"{nameof(tag)} length must be equal to or lower than {TagConsts.MaxNameLength}",
                    new[] { nameof(Tags) }
                );
            }
        }
    }
}
