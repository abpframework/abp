using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;
using Volo.CmsKit.Contents;

namespace Volo.CmsKit.Pages
{
    public class UpdatePageContentInputDto
    {
        [Required]
        [DynamicMaxLength(typeof(ContentConsts), nameof(ContentConsts.MaxValueLength))]
        public string Content { get; set; }
    }
}