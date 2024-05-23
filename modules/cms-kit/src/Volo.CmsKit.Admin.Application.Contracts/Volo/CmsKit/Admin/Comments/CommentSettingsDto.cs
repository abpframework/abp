using System;

namespace Volo.CmsKit.Admin.Comments;

[Serializable]
public class SettingsDto
{
    public bool CommentRequireApprovement { get; set; }
}
