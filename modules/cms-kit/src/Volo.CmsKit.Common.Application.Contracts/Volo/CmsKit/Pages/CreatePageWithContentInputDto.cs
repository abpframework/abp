using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;
using Volo.CmsKit.Contents;

namespace Volo.CmsKit.Pages
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