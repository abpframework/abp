using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace Volo.CmsKit.Comments
{
    public class UpdateCommentInput
    {
        [Required]
        [DynamicStringLength(typeof(CommentConsts), nameof(CommentConsts.MaxTextLength))]
        public string Text { get; set; }
    }
}
