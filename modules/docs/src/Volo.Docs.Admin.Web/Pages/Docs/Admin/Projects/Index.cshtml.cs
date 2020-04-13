using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Volo.Docs.Admin.Pages.Docs.Admin.Projects
{
    [Authorize(DocsAdminPermissions.Projects.Default)]
    public class IndexModel : DocsAdminPageModel
    {
        public ActionResult OnGet()
        {
            return Page();
        }
    }
}