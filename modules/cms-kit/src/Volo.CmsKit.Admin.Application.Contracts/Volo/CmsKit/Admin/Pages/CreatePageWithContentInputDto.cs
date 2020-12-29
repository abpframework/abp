using System.ComponentModel.DataAnnotations;

namespace Volo.CmsKit.Admin.Pages
{
    public class CreatePageWithContentInputDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Url { get; set; }

        public string Description { get; set; }
        
        [Required]
        public string Content { get; set; }
    }
}