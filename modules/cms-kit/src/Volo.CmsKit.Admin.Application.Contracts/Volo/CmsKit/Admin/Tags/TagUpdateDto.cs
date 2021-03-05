using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;
using Volo.CmsKit.Tags;

namespace Volo.CmsKit.Admin.Tags
{
    [Serializable]
    public class TagUpdateDto
    {
        [Required]
        [DynamicMaxLength(typeof(TagConsts), nameof(TagConsts.MaxNameLength))]
        public string Name { get; set; }
    }
}
