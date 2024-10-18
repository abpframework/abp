using System;

namespace Volo.CmsKit.Admin.Comments;

[Serializable]
public class CommentSettingsDto
{
    public bool CommentRequireApprovement { get; set; }
}
