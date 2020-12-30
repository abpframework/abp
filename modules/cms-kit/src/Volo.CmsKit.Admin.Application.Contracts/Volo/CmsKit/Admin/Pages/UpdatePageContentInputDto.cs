using System.ComponentModel.DataAnnotations;

namespace Volo.CmsKit.Admin.Pages
{
    public class UpdatePageContentInputDto
    {
        [Required]
        public string Content { get; set; }
    }
}