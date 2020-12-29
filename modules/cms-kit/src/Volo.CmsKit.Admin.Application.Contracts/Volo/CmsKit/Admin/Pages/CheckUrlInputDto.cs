using System.ComponentModel.DataAnnotations;

namespace Volo.CmsKit.Admin.Pages
{
    public class CheckUrlInputDto
    {
        [Required]
        public string Url { get; set; }
    }
}