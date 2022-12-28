using JetBrains.Annotations;
using System.Collections.Generic;

namespace Volo.CmsKit.Comments;

public class CmsKitCommentOptions
{
    [NotNull]
    public List<CommentEntityTypeDefinition> EntityTypes { get; } = new List<CommentEntityTypeDefinition>();

    /// <summary>
    /// Flag to enable/disable ReCaptcha for comment component.
    /// Default: false
    /// </summary>
    public bool IsRecaptchaEnabled { get; set; }
}
