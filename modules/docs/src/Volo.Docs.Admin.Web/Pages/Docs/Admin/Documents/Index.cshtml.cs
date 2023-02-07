using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Docs.Admin.Projects;

namespace Volo.Docs.Admin.Pages.Docs.Admin.Documents;

[Authorize(DocsAdminPermissions.Projects.Default)]
public class IndexModel : DocsAdminPageModel
{
    private readonly IProjectAdminAppService _projectAdminAppService;
    public List<ProjectWithoutDetailsDto> Projects { get; set; }

    public IndexModel(IProjectAdminAppService projectAdminAppService)
    {
        _projectAdminAppService = projectAdminAppService;
    }
    public virtual async Task<IActionResult> OnGet()
    {
        Projects = await _projectAdminAppService.GetListWithoutDetailsAsync();
        return Page();
    }
}
