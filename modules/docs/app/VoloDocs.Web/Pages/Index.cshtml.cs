using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Volo.Docs;
using Volo.Docs.Projects;

namespace VoloDocs.Web.Pages
{
    public class IndexModel : PageModel
    {
        public IReadOnlyList<ProjectDto> Projects { get; set; }

        private readonly DocsUiOptions _urlUiOptions;

        private readonly IProjectAppService _projectAppService;

        public IndexModel(IOptions<DocsUiOptions> urlOptions, IProjectAppService projectAppService)
        {
            _projectAppService = projectAppService;
            _urlUiOptions = urlOptions.Value;
        }

        public virtual async Task<IActionResult> OnGetAsync()
        {
            if (_urlUiOptions.SingleProjectMode.Enable)
            {
                return Redirect(GetUrlForProject());
            }
            var projects = await _projectAppService.GetListAsync();

            if (projects.Items.Count == 1)
            {
                return await RedirectToProjectAsync(projects.Items.First());
            }
            else if (projects.Items.Count > 1)
            {
                Projects = projects.Items;
            }

            return Page();
        }

        private async Task<IActionResult> RedirectToProjectAsync(ProjectDto project, string language = "en", string version = null)
        {
            var path = GetUrlForProject(project, language, version);
            return await Task.FromResult(Redirect(path));
        }

        //Eg: "/en/abp/latest"
        public string GetUrlForProject(ProjectDto project = null, string language = "en", string version = null)
        {
            var routeValues = new Dictionary<string, object> {
                { nameof(Volo.Docs.Pages.Documents.Project.IndexModel.Version), version ?? DocsAppConsts.Latest }
            };

            if (!_urlUiOptions.SingleProjectMode.Enable)
            {
                routeValues.Add(nameof(Volo.Docs.Pages.Documents.Project.IndexModel.ProjectName), project?.ShortName);
            }
            
            if(_urlUiOptions.MultiLanguageMode)
            {
                routeValues.Add(nameof(Volo.Docs.Pages.Documents.Project.IndexModel.LanguageCode), language);
            }
            
            return Url.Page("/Documents/Project/Index", routeValues);
        }
    }
}
