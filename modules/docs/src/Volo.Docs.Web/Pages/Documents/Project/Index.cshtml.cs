using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Docs.Documents;
using Volo.Docs.Formatting;
using Volo.Docs.Projects;

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

        public string ProjectFormat { get; private set; }

        public string DocumentNameWithExtension { get; private set; }

        public DocumentWithDetailsDto Document { get; private set; }

        public List<VersionInfo> Versions { get; private set; }

        public List<SelectListItem> VersionSelectItems { get; private set; }

        public NavigationWithDetailsDto Navigation { get; private set; }

        public VersionInfo LatestVersionInfo { get; private set; }

        private readonly IDocumentAppService _documentAppService;
        private readonly IDocumentConverterFactory _documentConverterFactory;
        private readonly IProjectAppService _projectAppService;

        public IndexModel(IDocumentAppService documentAppService, IDocumentConverterFactory documentConverterFactory, IProjectAppService projectAppService)
        {
            _documentAppService = documentAppService;
            _documentConverterFactory = documentConverterFactory;
            _projectAppService = projectAppService;
        }

        public async Task OnGet()
        {
            var project = await _projectAppService.FindByShortNameAsync(ProjectName);

            SetPageParams(project);

            await SetVersionAsync(project);

            await SetDocumentAsync();

            await SetNavigationAsync();
        }

        private async Task SetNavigationAsync()
        {
            Navigation = await _documentAppService.GetNavigationDocumentAsync(ProjectName, Version, false);
            Navigation.ConvertItems();
        }

        private void SetPageParams(ProjectDto project)
        {
            ProjectFormat = project.Format;

            if (DocumentName.IsNullOrWhiteSpace())
            {
                DocumentName = project.DefaultDocumentName;
            }

            DocumentNameWithExtension = DocumentName + "." + project.Format;
        }
        private static void SetLastVersionAsLatest(IReadOnlyList<VersionInfo> versions)
        {
            if (!versions.Any())
            {
                return;
            }

            versions[0].DisplayText = versions[0].Version + " - latest";
            versions[0].Version = "latest";
        }


        private async Task SetVersionAsync(ProjectDto project)
        {
            var versions = await _documentAppService.GetVersions(project.ShortName, project.DefaultDocumentName,
                project.ExtraProperties, project.DocumentStoreType, DocumentNameWithExtension);

            Versions = versions.Select(v => new VersionInfo(v, v)).ToList();

          //  SetLastVersionAsLatest(Versions);

            LatestVersionInfo = GetLatestVersion();

            if (string.Equals(Version, DocsAppConsts.LatestVersion, StringComparison.OrdinalIgnoreCase))
            {
                LatestVersionInfo.IsSelected = true;
                Version = LatestVersionInfo.Version;
            }
            else
            {
                var versionFromUrl = Versions.FirstOrDefault(v => v.Version == Version);
                if (versionFromUrl != null)
                {
                    versionFromUrl.IsSelected = true;
                    Version = versionFromUrl.Version;
                }
                else
                {
                    Versions.First().IsSelected = true;
                    Version = Versions.First().Version;
                }
            }

            VersionSelectItems = Versions.Select(v => new SelectListItem
            {
                Text = v.DisplayText,
                Value = "/documents/" + ProjectName + "/" + v.Version + "/" + DocumentName,
                Selected = v.IsSelected
            }).ToList();

        }

        private VersionInfo GetLatestVersion()
        {
            var latestVersion = Versions.First();

            latestVersion.DisplayText = $"{latestVersion.Version} - " + DocsAppConsts.LatestVersion;
            latestVersion.Version = latestVersion.Version;

            return latestVersion;
        }

        private async Task SetDocumentAsync()
        {
            Document = await _documentAppService.GetByNameAsync(ProjectName, DocumentNameWithExtension, Version, true);
            var converter = _documentConverterFactory.Create(Document.Format ?? ProjectFormat);
            var content = converter.NormalizeLinks(Document.Content, Document.Project.ShortName, Document.Version, Document.LocalDirectory);
            content = converter.Convert(content);

            content = HtmlNormalizer.ReplaceImageSources(content, Document.RawRootUrl, Document.LocalDirectory);
            content = HtmlNormalizer.ReplaceCodeBlocksLanguage(content, "language-C#", "language-csharp"); //todo find a way to make it on client in prismJS configuration (eg: map C# => csharp)
            Document.Content = content;
        }
    }
}