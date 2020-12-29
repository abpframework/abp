using System.ComponentModel.DataAnnotations;

namespace Volo.CmsKit.Admin.Pages
{
    public class CreatePageInputDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Url { get; set; }

        public string Description { get; set; }
    }
}