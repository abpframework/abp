using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Volo.Docs.Common.Projects;

namespace Volo.Docs.Pages.Documents
{
    public class IndexModel : PageModel
    {
        public string DocumentsUrlPrefix { get; set; }

        public IReadOnlyList<ProjectDto> Projects { get; set; }

        private readonly IProjectAppService _projectAppService;
        private readonly DocsUiOptions _uiOptions;

        public IndexModel(
            IProjectAppService projectAppService,
            IOptions<DocsUiOptions> urlOptions)
        {
            _projectAppService = projectAppService;
            _uiOptions = urlOptions.Value;
        }

        public virtual async Task<IActionResult> OnGetAsync()
        {
            if (_uiOptions.SingleProjectMode.Enable)
            {
                return Redirect($"/Documents/Project/Index?version={DocsAppConsts.Latest}");
            }

            var listResult = await _projectAppService.GetListAsync();

            if (listResult.Items.Count == 1)
            {
                return Redirect($"/Documents/{listResult.Items[0].ShortName}");
            }

            Projects = listResult.Items;

            return Page();
        }
        
        public string GetUrlForProject(ProjectDto project = null, string language = "en", string version = null)
        {
            var routeValues = new Dictionary<string, object> {
                { nameof(Project.IndexModel.Version), version ?? DocsAppConsts.Latest }
            };

            if (!_uiOptions.SingleProjectMode.Enable)
            {
                routeValues.Add(nameof(Project.IndexModel.ProjectName), project?.ShortName);
            }
            
            if (_uiOptions.MultiLanguageMode)
            {
                routeValues.Add(nameof(Project.IndexModel.LanguageCode), language);
            }
            
            return Url.Page("/Documents/Project/Index", routeValues);
        }
    }
}
