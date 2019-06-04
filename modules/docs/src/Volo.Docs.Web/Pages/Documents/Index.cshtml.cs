using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Volo.Docs.Projects;

namespace Volo.Docs.Pages.Documents
{
    public class IndexModel : PageModel
    {
        public string DocumentsUrlPrefix { get; set; }

        public IReadOnlyList<ProjectDto> Projects { get; set; }

        private readonly IProjectAppService _projectAppService;
        private readonly DocsUrlOptions _urlOptions;

        public IndexModel(
            IProjectAppService projectAppService,
            IOptions<DocsUrlOptions> urlOptions)
        {
            _projectAppService = projectAppService;
            _urlOptions = urlOptions.Value;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            DocumentsUrlPrefix = _urlOptions.GetFormattedRoutePrefix();

            var listResult = await _projectAppService.GetListAsync();

            if (listResult.Items.Count == 1)
            {
                return Redirect("./" + DocumentsUrlPrefix + listResult.Items[0].ShortName);
            }

            Projects = listResult.Items;

            return Page();
        }
    }
}