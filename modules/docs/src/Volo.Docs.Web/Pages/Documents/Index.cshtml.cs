using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Docs.Projects;

namespace Volo.Docs.Pages.Documents
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
            var result = await _projectAppService.GetListAsync();

            if (result.Items.Count == 1)
            {
                var project = result.Items[0];
                return RedirectToPage("./Project/Index", new
                {
                    projectName = project.ShortName,
                    version = DocsAppConsts.DefaultVersion.Version,
                    documentName = project.DefaultDocumentName
                });
            }

            Projects = result.Items;
            return Page();
        }
    }
}