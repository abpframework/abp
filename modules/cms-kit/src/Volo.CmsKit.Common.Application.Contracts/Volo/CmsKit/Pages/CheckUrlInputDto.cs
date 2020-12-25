using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace Volo.CmsKit.Pages
{
    public class CheckUrlInputDto
    {
        [Required]
        [DynamicMaxLength(typeof(PageConsts), nameof(PageConsts.MaxUrlLength))]
        public string Url { get; set; }
    }
}