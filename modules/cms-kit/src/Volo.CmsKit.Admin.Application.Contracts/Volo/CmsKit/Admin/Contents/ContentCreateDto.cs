using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;
using Volo.CmsKit.Contents;

namespace Volo.CmsKit.Admin.Contents
{
    public class ContentCreateDto
    {
        [Required]
        [DynamicMaxLength(typeof(ContentConsts), nameof(ContentConsts.MaxEntityTypeLength))]
        public string EntityType { get; set; }

        [Required]
        [DynamicMaxLength(typeof(ContentConsts), nameof(ContentConsts.MaxEntityIdLength))]
        public string EntityId { get; set; }

        [Required]
        [DynamicMaxLength(typeof(ContentConsts), nameof(ContentConsts.MaxValueLength))]
        public string Value { get; set; }
    }
}
