using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;
using Volo.CmsKit.Comments;

namespace Volo.CmsKit.Public.Comments
{
    public class CreateCommentInput
    {
        [Required]
        [DynamicStringLength(typeof(CommentConsts), nameof(CommentConsts.EntityTypeLength))]
        public string EntityType { get; set; }

        [Required]
        [DynamicStringLength(typeof(CommentConsts), nameof(CommentConsts.EntityIdLength))]
        public string EntityId { get; set; }

        [Required]
        [DynamicStringLength(typeof(CommentConsts), nameof(CommentConsts.MaxTextLength))]
        public string Text { get; set; }

        public Guid? RepliedCommentId { get; set; }
    }
}
