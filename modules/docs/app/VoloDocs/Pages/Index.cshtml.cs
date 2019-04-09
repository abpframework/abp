using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.Users;
using Volo.Docs;
using Volo.Docs.Projects;

namespace VoloDocs.Pages
{
    public class IndexModel : PageModel
    {
        public IReadOnlyList<ProjectDto> Projects { get; set; }
        public string CreateProjectLink { get; set; }

        private readonly IProjectAppService _projectAppService;
        private readonly ICurrentUser _currentUser;

        public IndexModel(IProjectAppService projectAppService, ICurrentUser currentUser)
        {
            _projectAppService = projectAppService;
            _currentUser = currentUser;
        }

        public async Task<IActionResult> OnGet()
        {
            CreateProjectLink = _currentUser.Id.HasValue
                ? "/Docs/Admin/Projects"
                : "/Account/Login?returnUrl=/Docs/Admin/Projects";

            Projects = (await _projectAppService.GetListAsync()).Items;

            if (Projects.Count == 1)
            {
                return RedirectToPage("./Documents/Project/Index", new
                {
                    projectName = Projects[0].ShortName,
                    version = DocsAppConsts.Latest,
                    documentName = Projects[0].DefaultDocumentName
                });
            }

            return Page();
        }
    }
}