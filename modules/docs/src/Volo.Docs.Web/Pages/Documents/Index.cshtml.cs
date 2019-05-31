using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Volo.Abp.Configuration;
using Volo.Docs.Projects;

namespace Volo.Docs.Pages.Documents
{
    public class IndexModel : PageModel
    {
        public string DocumentsUrlPrefix { get; set; }

        public IReadOnlyList<ProjectDto> Projects { get; set; }

        private readonly IProjectAppService _projectAppService;
        private readonly IConfigurationRoot _configuration;

        public IndexModel(IProjectAppService projectAppService, IConfigurationAccessor configurationAccessor)
        {
            _configuration = configurationAccessor.Configuration;
            _projectAppService = projectAppService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            DocumentsUrlPrefix = Convert.ToBoolean(_configuration["IncludeDocumentPrefixInUrl"]) ? "documents/" : "";

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