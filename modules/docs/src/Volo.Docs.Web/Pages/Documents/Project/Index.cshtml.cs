using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Docs.Documents;
using Volo.Docs.Formatting;

namespace Volo.Docs.Pages.Documents.Project
{
    public class IndexModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string ProjectName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Version { get; set; } = "";

        [BindProperty(SupportsGet = true)]
        public string DocumentName { get; set; }

        public DocumentWithDetailsDto Document { get; private set; }

        public List<VersionInfo> Versions { get; private set; }

        public NavigationWithDetailsDto Navigation { get; private set; }

        private readonly IDocumentAppService _documentAppService;
        private readonly IDocumentFormattingFactory _documentFormattingFactory;

        public IndexModel(IDocumentAppService documentAppService, IDocumentFormattingFactory documentFormattingFactory)
        {
            _documentAppService = documentAppService;
            _documentFormattingFactory = documentFormattingFactory;
        }

        public async Task OnGet()
        {
            Versions = (await _documentAppService.GetVersions(ProjectName, DocumentName))
                .Select(v => new VersionInfo(v, v))
                .ToList();


            var latestVersion = Versions.First();
            latestVersion.DisplayText = $"{latestVersion.Version} - latest";
            latestVersion.Version = latestVersion.Version;

            AddDefaultVersionIfNotContains();

            var versionFromUrl = Versions.FirstOrDefault(v => v.Version == Version);
            if (versionFromUrl != null)
            {
                versionFromUrl.IsSelected = true;
            }
            else if (string.Equals(Version, "latest", StringComparison.InvariantCultureIgnoreCase))
            {
                latestVersion.IsSelected = true;
            }
            else
            {
                Versions.First().IsSelected = true;
            }

            if (Version == null)
            {
                Version = Versions.Single(x => x.IsSelected).Version;
            }

            Document = await _documentAppService.GetByNameAsync(ProjectName, DocumentName, Version, true);
            var documentFormatting = _documentFormattingFactory.Create(Document.Format ?? "md");
            Document.Content = documentFormatting.Format(Document.Content);

            Navigation = await _documentAppService.GetNavigationDocumentAsync(ProjectName, Version, false);
            Navigation.ConvertItems();
        }

        private void AddDefaultVersionIfNotContains()
        {
            if (DocsWebConsts.DefaultVersion == null)
            {
                return;
            }

            if (Versions.Contains(DocsWebConsts.DefaultVersion))
            {
                return;
            }

            Versions.Insert(0, DocsWebConsts.DefaultVersion);
        }
    }
}