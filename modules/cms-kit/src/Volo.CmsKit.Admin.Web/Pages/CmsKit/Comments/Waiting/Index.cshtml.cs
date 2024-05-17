using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Volo.CmsKit.Admin.Web.Pages.CmsKit.Comments.Waiting;

public class IndexModel : CmsKitAdminPageModel
{
    public string EntityType { get; set; }

    public string Author { get; set; }

    public DateTime? CreationStartDate { get; set; }

    public DateTime? CreationEndDate { get; set; }
    public string IsApproved { get; set; }

}
