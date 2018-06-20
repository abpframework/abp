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

        public DocumentWithDetailsDto NavigationDocument { get; private set; }

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

            if (string.Equals(Version, "latest", StringComparison.OrdinalIgnoreCase) || !Versions.Exists(v => v.Version == Version))
            {
                Version = latestVersion.Version;
            }

            latestVersion.DisplayText = $"latest {latestVersion.Version}";
            latestVersion.Version = "latest";

            Document = await _documentAppService.GetByNameAsync(ProjectName, DocumentName, Version);
            var documentFormatting = _documentFormattingFactory.Create(Document.Format ?? "md");
            Document.Content = documentFormatting.Format(Document.Content);

            NavigationDocument = await _documentAppService.GetNavigationDocumentAsync(ProjectName, Version);
            var navigationDocumentFormatting = _documentFormattingFactory.Create(NavigationDocument.Format);
            NavigationDocument.Content = navigationDocumentFormatting.Format(NavigationDocument.Content);
        }

        public class VersionInfo
        {
            public string DisplayText { get; set; }

            public string Version { get; set; }

            public VersionInfo(string displayText, string version)
            {
                DisplayText = displayText;
                Version = version;
            }
        }
    }
}