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
        private readonly DocsOptions _options;

        public IndexModel(
            IProjectAppService projectAppService,
            IOptions<DocsOptions> urlOptions)
        {
            _projectAppService = projectAppService;
            _options = urlOptions.Value;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            DocumentsUrlPrefix = _options.RoutePrefix;

            var listResult = await _projectAppService.GetListAsync();

            if (listResult.Items.Count == 1)
            {
                return Redirect("." + DocumentsUrlPrefix + listResult.Items[0].ShortName);
            }

            Projects = listResult.Items;

            return Page();
        }
    }
}