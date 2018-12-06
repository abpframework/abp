using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Docs.Documents;
using Volo.Docs.HtmlConverting;
using Volo.Docs.Models;
using Volo.Docs.Projects;

namespace Volo.Docs.Pages.Documents.Project
{
    public class IndexModel : AbpPageModel
    {
        [BindProperty(SupportsGet = true)]
        public string ProjectName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Version { get; set; } = "";

        [BindProperty(SupportsGet = true)]
        public string DocumentName { get; set; }

        public string ProjectDisplayName { get; set; }

        public string ProjectFormat { get; private set; }

        public string DocumentNameWithExtension { get; private set; }

        public DocumentWithDetailsDto Document { get; private set; }

        public List<VersionInfo> Versions { get; private set; }

        public List<SelectListItem> VersionSelectItems { get; private set; }

        public NavigationWithDetailsDto Navigation { get; private set; }

        public VersionInfo LatestVersionInfo { get; private set; }

        private readonly IDocumentAppService _documentAppService;
        private readonly IDocumentToHtmlConverterFactory _documentToHtmlConverterFactory;
        private readonly IProjectAppService _projectAppService;

        public IndexModel(
            IDocumentAppService documentAppService, 
            IDocumentToHtmlConverterFactory documentToHtmlConverterFactory, 
            IProjectAppService projectAppService)
        {
            _documentAppService = documentAppService;
            _documentToHtmlConverterFactory = documentToHtmlConverterFactory;
            _projectAppService = projectAppService;
        }

        public async Task OnGet()
        {
            var project = await _projectAppService.GetByShortNameAsync(ProjectName);

            SetPageParams(project);

            await SetVersionAsync(project);
            await SetDocumentAsync(project);
            await SetNavigationAsync(project);
        }

        private async Task SetNavigationAsync(ProjectDto project)
        {
            try
            {
                var document = await _documentAppService.GetNavigationDocumentAsync(
                    new GetNavigationDocumentInput
                    {
                        ProjectId = project.Id,
                        Version = Version
                    }
                );

                Navigation = ObjectMapper.Map<DocumentWithDetailsDto, NavigationWithDetailsDto>(document);
            }
            catch (DocumentNotFoundException) //TODO: What if called on a remote service which may return 404
            {
                return;
            }

            Navigation.ConvertItems();
        }

        private void SetPageParams(ProjectDto project)
        {
            ProjectFormat = project.Format;
            ProjectDisplayName = project.Name;

            if (DocumentName.IsNullOrWhiteSpace())
            {
                DocumentName = project.DefaultDocumentName;
            }

            DocumentNameWithExtension = DocumentName + "." + project.Format;
        }

        private async Task SetVersionAsync(ProjectDto project)
        {
            var versionInfoDtos = await _projectAppService.GetVersionsAsync(project.Id);

            Versions = versionInfoDtos.Select(v => new VersionInfo(v.DisplayName, v.Name)).ToList();

            LatestVersionInfo = GetLatestVersion();

            if (string.Equals(Version, DocsAppConsts.Latest, StringComparison.OrdinalIgnoreCase))
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
                Value = CreateLink(LatestVersionInfo, v.Version, DocumentName),
                Selected = v.IsSelected
            }).ToList();
        }

        public string CreateLink(VersionInfo latestVersion, string version, string documentName = null)
        {
            if (latestVersion.Version == version)
            {
                version = DocsAppConsts.Latest;
            }

            var link = "/documents/" + ProjectName + "/" + version;

            if (documentName != null)
            {
                link += "/" + DocumentName;
            }

            return link;
        }

        private VersionInfo GetLatestVersion()
        {
            var latestVersion = Versions.First();

            latestVersion.DisplayText = $"{latestVersion.DisplayText} ({DocsAppConsts.Latest})";
            latestVersion.Version = latestVersion.Version;

            return latestVersion;
        }

        public string GetSpecificVersionOrLatest()
        {
            if (Document?.Version == null)
            {
                return DocsAppConsts.Latest;
            }

            return Document.Version == LatestVersionInfo.Version ?
                DocsAppConsts.Latest :
                Document.Version;
        }

        private async Task SetDocumentAsync(ProjectDto project)
        {
            try
            {
                if (DocumentNameWithExtension.IsNullOrWhiteSpace())
                {
                    Document = await _documentAppService.GetDefaultAsync(
                        new GetDefaultDocumentInput
                        {
                            ProjectId = project.Id,
                            Version = Version
                        }
                    );
                }
                else
                {
                    Document = await _documentAppService.GetAsync(
                        new GetDocumentInput
                        {
                            ProjectId = project.Id,
                            Name = DocumentNameWithExtension,
                            Version = Version
                        }
                    );
                }
            }
            catch (DocumentNotFoundException)
            {
                return;
            }
           
            var converter = _documentToHtmlConverterFactory.Create(Document.Format ?? ProjectFormat);

            var content = converter.NormalizeLinks(Document.Content, Document.Project.ShortName, GetSpecificVersionOrLatest(), Document.LocalDirectory);
            content = converter.Convert(content);

            content = HtmlNormalizer.ReplaceImageSources(content, Document.RawRootUrl, Document.LocalDirectory);
            content = HtmlNormalizer.ReplaceCodeBlocksLanguage(content, "language-C#", "language-csharp"); //todo find a way to make it on client in prismJS configuration (eg: map C# => csharp)

            Document.Content = content;
        }

    }
}