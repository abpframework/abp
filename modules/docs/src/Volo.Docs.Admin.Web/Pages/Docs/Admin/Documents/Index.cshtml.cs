using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Docs.Documents;

namespace Volo.Docs.Admin.Pages.Docs.Admin.Documents
{
    [Authorize(DocsAdminPermissions.Projects.Default)]
    public class IndexModel : DocsAdminPageModel
    {
        public virtual Task<IActionResult> OnGet()
        {
            return Task.FromResult<IActionResult>(Page());
        }
    }
}