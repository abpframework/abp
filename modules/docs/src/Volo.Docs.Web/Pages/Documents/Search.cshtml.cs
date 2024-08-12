using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Pagination;
using Volo.Abp.Data;
using Volo.Docs.Documents;
using Volo.Docs.GitHub.Documents.Version;
using Volo.Docs.HtmlConverting;
using Volo.Docs.Models;
using Volo.Docs.Projects;
using Volo.Docs.Utils;

namespace Volo.Docs.Pages.Documents
{
    public class SearchModel : PageModel
    {
        [BindProperty(SupportsGet = true)] public string ProjectName { get; set; }

        [BindProperty(SupportsGet = true)] public string Version { get; set; }

        [BindProperty(SupportsGet = true)] public string LanguageCode { get; set; }

        [BindProperty(SupportsGet = true)] public string KeyWord { get; set; }

        [BindProperty(SupportsGet = true)] public int CurrentPage { get; set; } = 1;

        public PagerModel PagerModel { get; set; }

        public ProjectDto Project { get; set; }

        private readonly IProjectAppService _projectAppService;
        private readonly IDocumentAppService _documentAppService;
        private readonly HtmlEncoder _encoder;
        private readonly DocsUiOptions _uiOptions;

        public SearchModel(IProjectAppService projectAppService,
            IDocumentAppService documentAppService,
            HtmlEncoder encoder, IOptions<DocsUiOptions> uiOptions)
        {
            _projectAppService = projectAppService;
            _documentAppService = documentAppService;
            _encoder = encoder;
            _uiOptions = uiOptions.Value;
        }

        public List<DocumentSearchOutput> SearchOutputs { get; set; } = new List<DocumentSearchOutput>();

        public virtual async Task<IActionResult> OnGetAsync(string keyword)
        {
            if (!await _documentAppService.FullSearchEnabledAsync())
            {
                return RedirectToPage("Index");
            }

            if (keyword.IsNullOrWhiteSpace())
            {
                return Page();
            }

            KeyWord = keyword;

            await SetProjectAsync();

            var output = await _projectAppService.GetVersionsAsync(Project.ShortName);

            var versions = output.Items.ToList();

            if (versions.Any() &&
                string.Equals(Version, DocsAppConsts.Latest, StringComparison.OrdinalIgnoreCase))
            {
                if ((!Project.HasProperty("GithubVersionProviderSource") ||
                     Project.GetProperty<GithubVersionProviderSource>("GithubVersionProviderSource") ==GithubVersionProviderSource.Releases) &&
                    !string.IsNullOrEmpty(Project.LatestVersionBranchName))
                {
                    Version = Project.LatestVersionBranchName;
                }
                else
                {
                    Version = (versions.FirstOrDefault(v => !SemanticVersionHelper.IsPreRelease(v.Name)) ?? versions.First()).Name;
                }
            }

            var pagedSearchOutputs = await _documentAppService.SearchAsync(new DocumentSearchInput
            {
                ProjectId = Project.Id,
                Context = KeyWord,
                LanguageCode = LanguageCode,
                Version = Version,
                MaxResultCount = 10,
                SkipCount = (CurrentPage - 1) * 10
            });

            SearchOutputs = pagedSearchOutputs.Items.ToList();

            PagerModel = new PagerModel(pagedSearchOutputs.TotalCount, 10, CurrentPage, 10, Url.Page("Search", new
            {
                ProjectName,
                Version,
                LanguageCode,
                KeyWord
            }));

            var highlightTag1 = Guid.NewGuid().ToString();
            var highlightTag2 = Guid.NewGuid().ToString();
            foreach (var searchOutput in SearchOutputs)
            {
                for (var i = 0; i < searchOutput.Highlight.Count; i++)
                {
                    searchOutput.Highlight[i] = _encoder
                        .Encode(searchOutput.Highlight[i].Replace("<highlight>", highlightTag1)
                            .Replace("</highlight>", highlightTag2))
                        .Replace(highlightTag1, "<highlight>").Replace(highlightTag2, "</highlight>");
                }
            }

            return Page();
        }

        private async Task SetProjectAsync()
        {
            if (!_uiOptions.SingleProjectMode.Enable)
            {
                Project = await _projectAppService.GetAsync(ProjectName);
                return;
            }

            var singleProjectName = ProjectName ?? _uiOptions.SingleProjectMode.ProjectName;
            if (!singleProjectName.IsNullOrWhiteSpace())
            {
                Project = await _projectAppService.GetAsync(singleProjectName);
                return;
            }

            var listResult = await _projectAppService.GetListAsync();
            if (listResult.Items.Count == 1)
            {
                Project = listResult.Items[0];
            }
        }
    }
}
