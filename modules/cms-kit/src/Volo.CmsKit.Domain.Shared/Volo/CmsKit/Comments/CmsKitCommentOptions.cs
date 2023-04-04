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

    /// <summary>
    /// Indicates the allowed external URLs, which can be included in a comment.
    /// If it's not specified, all external URLs will be allowed.
    /// </summary>
    public List<string> AllowedExternalUrls { get; set; } = new();
}
