using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;
using Volo.CmsKit.Contents;

namespace Volo.CmsKit.Pages
{
    public class CreatePageWithContentInputDto
    {
        [Required]
        [DynamicMaxLength(typeof(PageConsts), nameof(PageConsts.MaxTitleLength))]
        public string Title { get; set; }

        [Required]
        [DynamicMaxLength(typeof(PageConsts), nameof(PageConsts.MaxUrlLength))]
        public string Url { get; set; }

        [DynamicMaxLength(typeof(PageConsts), nameof(PageConsts.MaxDescriptionLength))]
        public string Description { get; set; }
        
        [Required]
        [DynamicMaxLength(typeof(ContentConsts), nameof(ContentConsts.MaxValueLength))]
        public string Content { get; set; }
    }
}