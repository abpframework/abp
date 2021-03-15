using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;
using Volo.CmsKit.Pages;

namespace Volo.CmsKit.Admin.Pages
{
    [Serializable]
    public class CreatePageInputDto
    {
        [Required]
        [DynamicMaxLength(typeof(PageConsts), nameof(PageConsts.MaxTitleLength))]
        public string Title { get; set; }

        [Required]
        [DynamicMaxLength(typeof(PageConsts), nameof(PageConsts.MaxSlugLength))]
        public string Slug { get; set; }
        
        [DynamicMaxLength(typeof(PageConsts), nameof(PageConsts.MaxContentLength))]
        public string Content { get; set; }
    }
}