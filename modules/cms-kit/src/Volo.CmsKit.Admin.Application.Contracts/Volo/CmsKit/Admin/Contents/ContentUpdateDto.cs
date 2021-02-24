using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;
using Volo.CmsKit.Contents;

namespace Volo.CmsKit.Admin.Contents
{
    [Serializable]
    public class ContentUpdateDto
    {
        [Required]
        [DynamicMaxLength(typeof(ContentConsts), nameof(ContentConsts.MaxValueLength))]
        public string Value { get; set; }
    }
}
