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

        public async Task<IActionResult> OnGetAsync()
        {
            var listResult = await _projectAppService.GetListAsync();

            if (listResult.Items.Count == 1)
            {
                return RedirectToPage("./Project/Index", new
                {
                    projectName = listResult.Items[0].ShortName,
                    version = DocsAppConsts.Latest,
                    languageCode = await _projectAppService.GetDefaultLanguageCode(listResult.Items[0].ShortName),
                    documentName = listResult.Items[0].DefaultDocumentName
                });
            }

            Projects = listResult.Items;

            return Page();
        }
    }
}