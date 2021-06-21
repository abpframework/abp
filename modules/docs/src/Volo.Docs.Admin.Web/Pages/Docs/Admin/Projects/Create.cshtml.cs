using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.Validation;
using Volo.Docs.Admin.Projects;
using Volo.Docs.Projects;

namespace Volo.Docs.Admin.Pages.Docs.Admin.Projects
{
    public class CreateModel : DocsAdminPageModel
    {
        [BindProperty]
        public CreateGithubProjectViewModel GithubProject { get; set; }

        private readonly IProjectAdminAppService _projectAppService;

        public List<SelectListItem> FormatTypes { get; set; }
            = new List<SelectListItem> {new SelectListItem("markdown", "md")};

        public CreateModel(IProjectAdminAppService projectAppService)
        {
            _projectAppService = projectAppService;
        }

        public virtual async Task<ActionResult> OnGetAsync(string source)
        {
            if (source != null && source.ToLowerInvariant() == "github")
            {
                GithubProject = new CreateGithubProjectViewModel();
                return Page();
            }
            else
            {
                throw new BusinessException("UnknowDocumentSourceExceptionMessage");
            }
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {
            if (GithubProject != null)
            {
                var dto = GetGithubProjectAsDto();
                await _projectAppService.CreateAsync(dto);
            }

            return NoContent();
        }

        public CreateProjectDto GetGithubProjectAsDto()
        {
            var dto = ObjectMapper.Map<CreateGithubProjectViewModel, CreateProjectDto>(GithubProject);

            dto.ExtraProperties = new Dictionary<string, object>
            {
                {nameof(GithubProject.GitHubRootUrl), GithubProject.GitHubRootUrl},
                {nameof(GithubProject.GitHubUserAgent), GithubProject.GitHubUserAgent},
                {nameof(GithubProject.GitHubAccessToken), GithubProject.GitHubAccessToken},
                {nameof(GithubProject.GithubVersionProviderSource), GithubProject.GithubVersionProviderSource},
                {nameof(GithubProject.VersionBranchPrefix), GithubProject.VersionBranchPrefix}
            };

            return dto;
        }

        public abstract class CreateProjectViewModelBase
        {
            [Required]
            [DynamicStringLength(typeof(ProjectConsts), nameof(ProjectConsts.MaxNameLength))]
            public string Name { get; set; }

            [Required]
            [DynamicStringLength(typeof(ProjectConsts), nameof(ProjectConsts.MaxShortNameLength))]
            [InputInfoText("ShortNameInfoText")]
            public string ShortName { get; set; }

            [Required]
            [SelectItems(nameof(FormatTypes))]
            public string Format { get; set; }

            [DynamicStringLength(typeof(ProjectConsts), nameof(ProjectConsts.MaxDefaultDocumentNameLength))]
            public string DefaultDocumentName { get; set; } = "Index";

            [DynamicStringLength(typeof(ProjectConsts), nameof(ProjectConsts.MaxNavigationDocumentNameLength))]
            public string NavigationDocumentName { get; set; } = "docs-nav.json";

            [DynamicStringLength(typeof(ProjectConsts), nameof(ProjectConsts.MaxParametersDocumentNameLength))]
            public string ParametersDocumentName { get; set; } = "docs-params.json";

            [DynamicStringLength(typeof(ProjectConsts), nameof(ProjectConsts.MaxVersionNameLength))]
            public string MinimumVersion { get; set; }

            public string MainWebsiteUrl { get; set; }

            [DynamicStringLength(typeof(ProjectConsts), nameof(ProjectConsts.MaxLatestVersionBranchNameLength))]
            public string LatestVersionBranchName { get; set; }

            [HiddenInput]
            public string DocumentStoreType { get; set; } = "GitHub";
        }

        public class CreateGithubProjectViewModel : CreateProjectViewModelBase
        {
            [DisplayOrder(10001)]
            [Required]
            [StringLength(256)]
            public string GitHubRootUrl { get; set; }

            [DisplayOrder(10001)]
            [StringLength(512)]
            public string GitHubAccessToken { get; set; }

            [DisplayOrder(10002)]
            [StringLength(64)]
            public string GitHubUserAgent { get; set; }

            [DisplayOrder(10003)]
            public GithubVersionProviderSource GithubVersionProviderSource { get; set; } = GithubVersionProviderSource.Releases;

            [DisplayOrder(10004)]
            [StringLength(64)]
            public string VersionBranchPrefix { get; set; }
        }
    }
}
