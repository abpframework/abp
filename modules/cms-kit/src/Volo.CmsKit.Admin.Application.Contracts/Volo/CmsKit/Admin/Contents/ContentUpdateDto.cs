using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;
using Volo.CmsKit.Contents;

namespace Volo.CmsKit.Admin.Contents
{
    public class ContentUpdateDto
    {
        [Required]
        [DynamicMaxLength(typeof(ContentConsts), nameof(ContentConsts.MaxValueLength))]
        public string Value { get; set; }
    }
}
