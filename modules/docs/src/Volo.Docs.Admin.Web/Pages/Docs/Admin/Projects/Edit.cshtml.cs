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

        public async Task<ActionResult> OnGetAsync(Guid id)
        {
            var project = await _projectAppService.GetAsync(id);

            if (project.DocumentStoreType == "GitHub")
            {
                SetGithubProjectFromDto(project);
                return Page();
            }

            throw new BusinessException("UnknowDocumentSourceExceptionMessage");
        }

        public async Task<IActionResult> OnPostAsync()
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
                {nameof(GithubProject.GitHubAccessToken), GithubProject.GitHubAccessToken}
            };

            return dto;
        }

        public void SetGithubProjectFromDto(ProjectDto dto)
        {
            GithubProject = ObjectMapper.Map<ProjectDto,EditGithubProjectViewModel>(dto);

            GithubProject.GitHubAccessToken = (string) dto.ExtraProperties[nameof(GithubProject.GitHubAccessToken)];
            GithubProject.GitHubRootUrl = (string) dto.ExtraProperties[nameof(GithubProject.GitHubRootUrl)];
            GithubProject.GitHubUserAgent = (string) dto.ExtraProperties[nameof(GithubProject.GitHubUserAgent)];
        }

        public abstract class EditProjectViewModelBase
        {
            [Required]
            [HiddenInput]
            public Guid Id { get; set; }

            [Required]
            [StringLength(ProjectConsts.MaxNameLength)]
            public string Name { get; set; }

            [Required]
            [SelectItems(nameof(FormatTypes))]
            public string Format { get; set; }

            [StringLength(ProjectConsts.MaxDefaultDocumentNameLength)]
            public string DefaultDocumentName { get; set; }

            [StringLength(ProjectConsts.MaxNavigationDocumentNameLength)]
            public string NavigationDocumentName { get; set; }

            [StringLength(ProjectConsts.MaxParametersDocumentNameLength)]
            public string ParametersDocumentName { get; set; }

            [StringLength(ProjectConsts.MaxVersionNameLength)]
            public string MinimumVersion { get; set; }

            public string MainWebsiteUrl { get; set; }

            [StringLength(ProjectConsts.MaxLatestVersionBranchNameLength)]
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
        }
    }
}