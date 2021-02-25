﻿using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;
using Volo.CmsKit.Contents;
using Volo.CmsKit.Pages;

namespace Volo.CmsKit.Admin.Pages
{
    [Serializable]
    public class UpdatePageInputDto
    {
        [Required]
        [DynamicMaxLength(typeof(PageConsts), nameof(PageConsts.MaxTitleLength))]
        public string Title { get; set; }

        [Required]
        [DynamicMaxLength(typeof(PageConsts), nameof(PageConsts.MaxSlugLength))]
        public string Slug { get; set; }

        [DynamicMaxLength(typeof(PageConsts), nameof(PageConsts.MaxDescriptionLength))]
        public string Description { get; set; }
    }
}