using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Docs.Admin.Documents;
using Volo.Docs.Admin.Projects;

namespace Volo.Docs.Admin.Pages.Docs.Admin.Documents;

[Authorize(DocsAdminPermissions.Documents.Default)]
public class IndexModel : DocsAdminPageModel
{
    private readonly IDocumentAdminAppService _documentAdminAppService;
    public List<ProjectWithoutDetailsDto> Projects { get; set; }

    public IndexModel(IDocumentAdminAppService documentAdminAppService)
    {
        _documentAdminAppService = documentAdminAppService;
    }
    public virtual async Task<IActionResult> OnGet()
    {
        Projects = await _documentAdminAppService.GetProjectsAsync();
        return Page();
    }
}
