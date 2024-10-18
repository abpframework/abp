using System;
using System.Collections.Generic;
using System.Text;

namespace Volo.CmsKit.Admin.Comments;

[Serializable]
public class CommentApprovalDto
{
    public bool IsApproved { get; set; }
}
