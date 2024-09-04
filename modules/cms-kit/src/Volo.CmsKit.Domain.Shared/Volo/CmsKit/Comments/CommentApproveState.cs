using System;

namespace Volo.CmsKit.Comments;

[Flags]
public enum CommentApproveState
{
    All = 0,
    Approved = 1,
    Disapproved = 2,
    Waiting = 4
}
