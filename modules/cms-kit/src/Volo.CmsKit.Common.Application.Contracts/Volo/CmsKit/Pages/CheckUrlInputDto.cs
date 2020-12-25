using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace Volo.CmsKit.Pages
{
    public class CheckUrlInputDto
    {
        [Required]
        public string Url { get; set; }
    }
}