using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Volo.Docs.Admin.Pages.Docs.Admin.Projects;

[Authorize(DocsAdminPermissions.Projects.Default)]
public class ManagePdfFiles : DocsAdminPageModel
{
    public virtual Task<IActionResult> OnGet()
    {
        return Task.FromResult<IActionResult>(Page());
    }
}