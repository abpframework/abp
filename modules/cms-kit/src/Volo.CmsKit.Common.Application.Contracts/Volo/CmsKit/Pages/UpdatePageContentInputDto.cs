using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;
using Volo.CmsKit.Contents;

namespace Volo.CmsKit.Pages
{
    public class UpdatePageContentInputDto
    {
        [Required]
        public string Content { get; set; }
    }
}