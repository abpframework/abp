using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;
using Volo.CmsKit.Comments;

namespace Volo.CmsKit.Public.Comments;

[Serializable]
public class UpdateCommentInput : ExtensibleObject, IHasConcurrencyStamp
{
    [Required]
    [DynamicStringLength(typeof(CommentConsts), nameof(CommentConsts.MaxTextLength))]
    public string Text { get; set; }

    public string ConcurrencyStamp { get; set; }
    
    public Guid? CaptchaToken { get; set; }
    
    public int CaptchaAnswer { get; set; }
}
