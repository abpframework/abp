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
    public class EditModel : DocsAdminPageModel
    {
        [BindProperty]
        public EditGithubProjectViewModel GithubProject { get; set; }

        private readonly IProjectAdminAppService _projectAppService;

        public List<SelectListItem> FormatTypes { get; set; }
            = new List<SelectListItem> { new SelectListItem("markdown", "md") };

        public EditModel(IProjectAdminAppService projectAppService)
        {
            _projectAppService = projectAppService;
        }

        public virtual async Task<ActionResult> OnGetAsync(Guid id)
        {
            var project = await _projectAppService.GetAsync(id);

            if (project.DocumentStoreType == "GitHub")
            {
                SetGithubProjectFromDto(project);
                return Page();
            }

            throw new BusinessException("UnknowDocumentSourceExceptionMessage");
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {
            if (GithubProject != null)
            {
                var dto = GetGithubProjectAsDto();
                await _projectAppService.UpdateAsync(GithubProject.Id, dto);
            }

            return NoContent();
        }

        public UpdateProjectDto GetGithubProjectAsDto()
        {
            var dto = ObjectMapper.Map<EditGithubProjectViewModel, UpdateProjectDto>(GithubProject);

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

        public void SetGithubProjectFromDto(ProjectDto dto)
        {
            GithubProject = ObjectMapper.Map<ProjectDto,EditGithubProjectViewModel>(dto);

            GithubProject.GitHubAccessToken = (string) dto.ExtraProperties[nameof(GithubProject.GitHubAccessToken)];
            GithubProject.GitHubRootUrl = (string) dto.ExtraProperties[nameof(GithubProject.GitHubRootUrl)];
            GithubProject.GitHubUserAgent = (string) dto.ExtraProperties[nameof(GithubProject.GitHubUserAgent)];

            if (dto.ExtraProperties.ContainsKey(nameof(GithubProject.GithubVersionProviderSource)))
            {
                GithubProject.GithubVersionProviderSource = (GithubVersionProviderSource) (long) dto.ExtraProperties[nameof(GithubProject.GithubVersionProviderSource)];
            }

            if (dto.ExtraProperties.ContainsKey(nameof(GithubProject.VersionBranchPrefix)))
            {
                GithubProject.VersionBranchPrefix = (string) dto.ExtraProperties[nameof(GithubProject.VersionBranchPrefix)];
            }
        }

        public abstract class EditProjectViewModelBase
        {
            [Required]
            [HiddenInput]
            public Guid Id { get; set; }

            [Required]
            [DynamicStringLength(typeof(ProjectConsts), nameof(ProjectConsts.MaxNameLength))]
            public string Name { get; set; }

            [Required]
            [SelectItems(nameof(FormatTypes))]
            public string Format { get; set; }

            [DynamicStringLength(typeof(ProjectConsts), nameof(ProjectConsts.MaxDefaultDocumentNameLength))]
            public string DefaultDocumentName { get; set; }

            [DynamicStringLength(typeof(ProjectConsts), nameof(ProjectConsts.MaxNavigationDocumentNameLength))]
            public string NavigationDocumentName { get; set; }

            [DynamicStringLength(typeof(ProjectConsts), nameof(ProjectConsts.MaxParametersDocumentNameLength))]
            public string ParametersDocumentName { get; set; }

            [DynamicStringLength(typeof(ProjectConsts), nameof(ProjectConsts.MaxVersionNameLength))]
            public string MinimumVersion { get; set; }

            public string MainWebsiteUrl { get; set; }

            [DynamicStringLength(typeof(ProjectConsts), nameof(ProjectConsts.MaxLatestVersionBranchNameLength))]
            public string LatestVersionBranchName { get; set; }
        }

        public class EditGithubProjectViewModel : EditProjectViewModelBase
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
