using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Docs.Documents;
using Volo.Docs.HtmlConverting;
using Volo.Docs.Models;
using Volo.Docs.Projects;

namespace Volo.Docs.Pages.Documents
{
    public class SearchModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string ProjectName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Version { get; set; }

        [BindProperty(SupportsGet = true)]
        public string LanguageCode { get; set; }

        [BindProperty(SupportsGet = true)]
        public string KeyWord { get; set; }

        public ProjectDto Project { get; set; }

        private readonly IProjectAppService _projectAppService;
        private readonly IDocumentAppService _documentAppService;

        public SearchModel(IProjectAppService projectAppService, 
            IDocumentAppService documentAppService)
        {
            _projectAppService = projectAppService;
            _documentAppService = documentAppService;
        }

        public List<DocumentSearchOutput> SearchOutputs { get; set; } = new List<DocumentSearchOutput>();

        public virtual async Task<IActionResult> OnGetAsync(string keyword)
        {
            if (!await _documentAppService.FullSearchEnabledAsync())
            {
                return RedirectToPage("Index");
            }
            
            KeyWord = keyword;

            Project = await _projectAppService.GetAsync(ProjectName);

            var output = await _projectAppService.GetVersionsAsync(Project.ShortName);

            var versions = output.Items.ToList();

            if (versions.Any() && string.Equals(Version, DocsAppConsts.Latest, StringComparison.OrdinalIgnoreCase))
            {
                Version = versions.First().Name;
            }

            SearchOutputs = await _documentAppService.SearchAsync(new DocumentSearchInput
            {
                ProjectId = Project.Id,
                Context = KeyWord,
                LanguageCode = LanguageCode,
                Version = Version
            });

            return Page();
        }
    }
}