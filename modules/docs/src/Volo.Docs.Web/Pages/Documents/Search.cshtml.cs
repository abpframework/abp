using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        public ProjectDto Project { get; set; }

        private readonly IProjectAppService _projectAppService;
        private readonly IDocumentAppService _documentAppService;
        private readonly HtmlEncoder _encoder;

        public SearchModel(IProjectAppService projectAppService,
            IDocumentAppService documentAppService,
            HtmlEncoder encoder)
        {
            _projectAppService = projectAppService;
            _documentAppService = documentAppService;
            _encoder = encoder;
        }

        public List<DocumentSearchOutput> SearchOutputs { get; set; } = new List<DocumentSearchOutput>();

        public virtual async Task<IActionResult> OnGetAsync(string keyword)
        {
            if (!await _documentAppService.FullSearchEnabledAsync())
            {
                return RedirectToPage("Index");
            }

            KeyWord = keyword;

            Project = await _projectAppService.GetAsync(ProjectName);

            var output = await _projectAppService.GetVersionsAsync(Project.ShortName);

            var versions = output.Items.ToList();

            if (versions.Any() &&
                string.Equals(Version, DocsAppConsts.Latest, StringComparison.OrdinalIgnoreCase))
            {
                if ((!Project.ExtraProperties.ContainsKey("GithubVersionProviderSource") ||
                     (GithubVersionProviderSource) (long) Project.ExtraProperties["GithubVersionProviderSource"] ==GithubVersionProviderSource.Releases) &&
                    !string.IsNullOrEmpty(Project.LatestVersionBranchName))
                {
                    Version = Project.LatestVersionBranchName;
                }
                else
                {
                    Version = (versions.FirstOrDefault(v => !SemanticVersionHelper.IsPreRelease(v.Name)) ?? versions.First()).Name;
                }
            }

            SearchOutputs = await _documentAppService.SearchAsync(new DocumentSearchInput
            {
                ProjectId = Project.Id,
                Context = KeyWord,
                LanguageCode = LanguageCode,
                Version = Version
            });

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
    }
}
