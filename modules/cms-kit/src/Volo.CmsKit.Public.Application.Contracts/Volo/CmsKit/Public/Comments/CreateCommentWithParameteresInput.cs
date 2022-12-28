using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;
using Volo.CmsKit.Comments;

namespace Volo.CmsKit.Public.Comments;

[Serializable]
public class CreateCommentWithParameteresInput
{
    [Required]
    [DynamicStringLength(typeof(CommentConsts), nameof(CommentConsts.MaxTextLength))]
    public string Text { get; set; }

    [Required]
    public string EntityType { get; set; }
    
    [Required]
    public string EntityId { get; set; }

    public Guid? RepliedCommentId { get; set; }
    
    public Guid? CaptchaToken { get; set; }
    
    public int CaptchaAnswer { get; set; }
}
