using System;
using Volo.CmsKit.Comments;

namespace Volo.CmsKit.Admin.Web.Pages.CmsKit.Comments;

public class IndexModel : CmsKitAdminPageModel
{
    public string EntityType { get; set; }

    public string Author { get; set; }

    public DateTime? CreationStartDate { get; set; } = null;

    public DateTime? CreationEndDate { get; set; }
    
    public CommentApproveState CommentApproveState { get; set; }
}
