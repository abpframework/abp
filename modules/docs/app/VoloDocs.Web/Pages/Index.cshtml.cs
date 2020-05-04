using System;
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
        public string GetUrlForProject(ProjectDto project, string language = "en", string version = null)
        {
            return "." +
                   _urlUiOptions.RoutePrefix.EnsureStartsWith('/').EnsureEndsWith('/') +
                   language.EnsureEndsWith('/') +
                   project.ShortName.EnsureEndsWith('/') +
                   (version ?? DocsAppConsts.Latest);
        }
    }
}
