using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Docs;
using Volo.Docs.Projects;

namespace VoloDocs.Web.Pages
{
    public class IndexModel : PageModel
    {
        public IReadOnlyList<ProjectDto> Projects { get; set; }

        private readonly IProjectAppService _projectAppService;

        public IndexModel(IProjectAppService projectAppService)
        {
            _projectAppService = projectAppService;
        }

        public async Task<IActionResult> OnGet()
        {
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