using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Localization;
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

        [BindProperty(SupportsGet = true)]
        public string LanguageCode { get; set; }

        public bool DocumentFound { get; set; } = true;

        public string DefaultLanguageCode { get; set; }

        public ProjectDto Project { get; set; }

        public LanguageConfig LanguageConfig { get; set; }

        public List<SelectListItem> LanguageSelectListItems { get; set; }

        public string DocumentNameWithExtension { get; private set; }

        public DocumentWithDetailsDto Document { get; private set; }

        public List<SelectListItem> VersionSelectItems { get; private set; }

        public List<SelectListItem> ProjectSelectItems { get; private set; }

        public NavigationNode Navigation { get; private set; }

        public VersionInfoViewModel LatestVersionInfo { get; private set; }

        public string DocumentsUrlPrefix { get; set; }

        public bool ShowProjectsCombobox { get; set; }

        public bool DocumentLanguageIsDifferent { get; set; }

        public DocumentParametersDto DocumentPreferences { get; set; }

        public DocumentRenderParameters UserPreferences { get; set; } = new DocumentRenderParameters();

        private readonly IDocumentAppService _documentAppService;
        private readonly IDocumentToHtmlConverterFactory _documentToHtmlConverterFactory;
        private readonly IProjectAppService _projectAppService;
        private readonly IDocumentSectionRenderer _documentSectionRenderer;
        private readonly DocsUiOptions _uiOptions;

        public IndexModel(
            IDocumentAppService documentAppService,
            IDocumentToHtmlConverterFactory documentToHtmlConverterFactory,
            IProjectAppService projectAppService,
            IOptions<DocsUiOptions> options,
            IDocumentSectionRenderer documentSectionRenderer)
        {
            ObjectMapperContext = typeof(DocsWebModule);

            _documentAppService = documentAppService;
            _documentToHtmlConverterFactory = documentToHtmlConverterFactory;
            _projectAppService = projectAppService;
            _documentSectionRenderer = documentSectionRenderer;
            _uiOptions = options.Value;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                return await SetPageAsync();
            }
            catch (DocumentNotFoundException exception)
            {
                Logger.LogWarning(exception.Message);

                DocumentFound = false;
                return Page();
            }
        }

        private async Task<IActionResult> SetPageAsync()
        {
            DocumentsUrlPrefix = _uiOptions.RoutePrefix;
            ShowProjectsCombobox = _uiOptions.ShowProjectsCombobox;

            try
            {
                await SetProjectAsync();
            }
            catch (EntityNotFoundException e)
            {
                Logger.LogWarning(e.Message);
                return NotFound();
            }

            if (ShowProjectsCombobox)
            {
                await SetProjectsAsync();
            }

            await SetVersionAsync();
            await SetLanguageList();

            if (IsDefaultDocument())
            {
                return RedirectToDefaultDocument();
            }

            if (!CheckLanguage())
            {
                return RedirectToDefaultLanguage();
            }

            if (IsDocumentCultureDifferentThanCurrent())
            {
                return ReloadPageWithCulture();
            }

            await SetDocumentAsync();
            await SetNavigationAsync();
            SetLanguageSelectListItems();

            return Page();
        }

        private bool IsDocumentCultureDifferentThanCurrent()
        {
            try
            {
                var newCulture = CultureInfo.GetCultureInfo(LanguageCode);

                return CultureInfo.CurrentCulture.Name != newCulture.Name;
            }
            catch (CultureNotFoundException)
            {
                return false;
            }
        }

        private bool IsDefaultDocument()
        {
            return DocumentName == Project.DefaultDocumentName;
        }

        private async Task SetProjectAsync()
        {
            Project = await _projectAppService.GetAsync(ProjectName);
        }

        private async Task SetLanguageList()
        {
            LanguageConfig = await _projectAppService.GetLanguageListAsync(ProjectName, Version);
            SetDefaultLanguageCode();
        }

        private void SetDefaultLanguageCode()
        {
            DefaultLanguageCode = (LanguageConfig.Languages.FirstOrDefault(l => l.IsDefault) ?? LanguageConfig.Languages.First()).Code;
        }

        private bool CheckLanguage()
        {
            return LanguageConfig.Languages.Any(l => l.Code == LanguageCode);
        }

        private IActionResult ReloadPageWithCulture()
        {
            var returnUrl = DocumentsUrlPrefix + LanguageCode + "/" + ProjectName + "/"
                            + (LatestVersionInfo.IsSelected ? DocsAppConsts.Latest : Version) + "/" +
                            DocumentName;

            return Redirect("/Abp/Languages/Switch?culture=" + LanguageCode + "&uiCulture=" + LanguageCode + "&returnUrl=" + returnUrl);
        }

        private IActionResult RedirectToDefaultLanguage()
        {
            return RedirectToPage(new
            {
                projectName = ProjectName,
                version = (LatestVersionInfo.IsSelected ? DocsAppConsts.Latest : Version),
                languageCode = DefaultLanguageCode,
                documentName = DocumentName
            });
        }

        private IActionResult RedirectToDefaultDocument()
        {
            return RedirectToPage(new
            {
                projectName = ProjectName,
                version = (LatestVersionInfo.IsSelected ? DocsAppConsts.Latest : Version),
                documentName = "",
                languageCode = DefaultLanguageCode
            });
        }

        private async Task SetProjectsAsync()
        {
            var projects = await _projectAppService.GetListAsync();

            ProjectSelectItems = projects.Items.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id != Project.Id ? DocumentsUrlPrefix + LanguageCode + "/" + p.ShortName + "/" + DocsAppConsts.Latest : null,
                Selected = p.Id == Project.Id
            }).ToList();
        }

        private async Task SetVersionAsync()
        {
            //TODO: Needs refactoring
            if (string.IsNullOrWhiteSpace(Version))
            {
                Version = DocsAppConsts.Latest;
            }

            var output = await _projectAppService.GetVersionsAsync(Project.ShortName);
            var versions = output.Items
                .Select(v => new VersionInfoViewModel(v.DisplayName, v.Name))
                .ToList();

            if (versions.Any())
            {
                LatestVersionInfo = versions.First();
                LatestVersionInfo.DisplayText = $"{LatestVersionInfo.DisplayText} ({DocsAppConsts.Latest})";

                if (string.Equals(Version, DocsAppConsts.Latest, StringComparison.OrdinalIgnoreCase))
                {
                    LatestVersionInfo.IsSelected = true;
                    Version = LatestVersionInfo.Version;
                }
                else
                {
                    var versionFromUrl = versions.FirstOrDefault(v => v.Version == Version);
                    if (versionFromUrl != null)
                    {
                        versionFromUrl.IsSelected = true;
                        Version = versionFromUrl.Version;
                    }
                    else
                    {
                        versions.First().IsSelected = true;
                        Version = versions.First().Version;
                    }
                }
            }
            else
            {
                LatestVersionInfo = new VersionInfoViewModel(
                    $"{DocsAppConsts.Latest}",
                    DocsAppConsts.Latest,
                    true);
            }

            VersionSelectItems = versions.Select(v => new SelectListItem
            {
                Text = v.DisplayText,
                Value = CreateVersionLink(LatestVersionInfo, v.Version, DocumentName),
                Selected = v.IsSelected
            }).ToList();
        }

        private async Task SetNavigationAsync()
        {
            try
            {
                Navigation = await _documentAppService.GetNavigationAsync(
                    new GetNavigationDocumentInput
                    {
                        ProjectId = Project.Id,
                        LanguageCode = LanguageCode,
                        Version = Version
                    }
                );
            }
            catch (DocumentNotFoundException) //TODO: What if called on a remote service which may return 404
            {
                return;
            }
        }

        public string CreateVersionLink(VersionInfoViewModel latestVersion, string version, string documentName = null)
        {
            if (latestVersion == null || latestVersion.Version == version)
            {
                version = DocsAppConsts.Latest;
            }

            var link = DocumentsUrlPrefix + LanguageCode + "/" + ProjectName + "/" + version;

            if (documentName != null)
            {
                link += "/" + DocumentName;
            }

            return link;
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

        private async Task SetDocumentAsync()
        {
            DocumentNameWithExtension = DocumentName + "." + Project.Format;

            try
            {
                if (DocumentName.IsNullOrWhiteSpace())
                {
                    Document = await _documentAppService.GetDefaultAsync(
                        new GetDefaultDocumentInput
                        {
                            ProjectId = Project.Id,
                            LanguageCode = LanguageCode,
                            Version = Version
                        }
                    );
                }
                else
                {
                    Document = await _documentAppService.GetAsync(
                        new GetDocumentInput
                        {
                            ProjectId = Project.Id,
                            Name = DocumentNameWithExtension,
                            LanguageCode = LanguageCode,
                            Version = Version
                        }
                    );
                }
            }
            catch (DocumentNotFoundException)
            {
                if (LanguageCode != DefaultLanguageCode)
                {
                    Document = await _documentAppService.GetAsync(
                        new GetDocumentInput
                        {
                            ProjectId = Project.Id,
                            Name = DocumentNameWithExtension,
                            LanguageCode = DefaultLanguageCode,
                            Version = Version
                        }
                    );

                    DocumentLanguageIsDifferent = true;
                }
                else
                {
                    throw;
                }
            }

            await ConvertDocumentContentToHtmlAsync();
        }

        private void SetLanguageSelectListItems()
        {
            LanguageSelectListItems = new List<SelectListItem>();

            foreach (var language in LanguageConfig.Languages)
            {
                LanguageSelectListItems.Add(
                    new SelectListItem(
                        language.DisplayName,
                        DocumentsUrlPrefix + language.Code + "/" + Project.ShortName + "/" + (LatestVersionInfo.IsSelected ? DocsAppConsts.Latest : Version) + "/" + DocumentName,
                        language.Code == LanguageCode
                        )
                    );
            }
        }

        private async Task ConvertDocumentContentToHtmlAsync()
        {
            await SetDocumentPreferencesAsync();
            SetUserPreferences();

            var partialTemplates = await GetDocumentPartialTemplatesAsync();

            Document.Content = await _documentSectionRenderer.RenderAsync(Document.Content, UserPreferences, partialTemplates);

            var converter = _documentToHtmlConverterFactory.Create(Document.Format ?? Project.Format);
            var content = converter.Convert(Project, Document, GetSpecificVersionOrLatest(), LanguageCode);

            content = HtmlNormalizer.ReplaceImageSources(
                content,
                Document.RawRootUrl,
                Document.LocalDirectory
            );

            //todo find a way to make it on client in prismJS configuration (eg: map C# => csharp)
            content = HtmlNormalizer.ReplaceCodeBlocksLanguage(
                content,
                "language-C#",
                "language-csharp"
            );

            Document.Content = content;
        }

        private async Task<List<DocumentPartialTemplateContent>> GetDocumentPartialTemplatesAsync()
        {
            var partialTemplatesInDocument = await _documentSectionRenderer.GetPartialTemplatesInDocumentAsync(Document.Content);

            if (!partialTemplatesInDocument?.Any(t => t.Parameters != null) ?? true)
            {
                return null;
            }

            foreach (var partialTemplates in partialTemplatesInDocument)
            {
                foreach (var parameter in partialTemplates.Parameters)
                {
                    if (!UserPreferences.ContainsKey(parameter.Key))
                    {
                        UserPreferences.Add(parameter.Key, parameter.Value);
                    }
                    else
                    {
                        UserPreferences[parameter.Key] = parameter.Value;
                    }
                }
            }

            var contents = new List<DocumentPartialTemplateContent>();

            foreach (var partialTemplate in partialTemplatesInDocument)
            {
                var content = (await _documentAppService.GetAsync(new GetDocumentInput
                {
                    LanguageCode = LanguageCode,
                    Name = partialTemplate.Path,
                    ProjectId = Project.Id,
                    Version = Version
                })).Content;

                contents.Add(new DocumentPartialTemplateContent
                {
                    Path = partialTemplate.Path,
                    Content = content
                });
            }

            return contents;
        }

        private void SetUserPreferences()
        {
            UserPreferences.Add("Document_Language_Code", LanguageCode);
            UserPreferences.Add("Document_Version", Version);

            var cookie = Request.Cookies["AbpDocsPreferences"];

            if (cookie != null)
            {
                var keyValues = cookie.Split("|");

                foreach (var keyValue in keyValues)
                {
                    if (keyValue.Split("=").Length < 2)
                    {
                        continue;
                    }

                    var key = keyValue.Split("=")[0];
                    var value = keyValue.Split("=")[1];

                    UserPreferences.Add(key, value);
                    UserPreferences.Add(key + "_Value", DocumentPreferences?.Parameters?.FirstOrDefault(p => p.Name == key)
                        ?.Values.FirstOrDefault(v => v.Key == value).Value);
                }
            }

            var query = Request.Query;

            foreach (var (key, value) in query)
            {
                if (UserPreferences.ContainsKey(key))
                {
                    UserPreferences.Remove(key);
                    UserPreferences.Remove(key + "_Value");
                }

                UserPreferences.Add(key, value);
                UserPreferences.Add(key + "_Value",
                    DocumentPreferences?.Parameters?.FirstOrDefault(p => p.Name == key)?.Values
                        .FirstOrDefault(v => v.Key == value).Value);
            }

            if (DocumentPreferences?.Parameters == null)
            {
                return;
            }

            foreach (var parameter in DocumentPreferences.Parameters)
            {
                if (!UserPreferences.ContainsKey(parameter.Name))
                {
                    UserPreferences.Add(parameter.Name, parameter.Values.FirstOrDefault().Key);
                    UserPreferences.Add(parameter.Name + "_Value", parameter.Values.FirstOrDefault().Value);
                }
            }

        }

        public async Task SetDocumentPreferencesAsync()
        {
            var projectParameters = await _documentAppService.GetParametersAsync(
                    new GetParametersDocumentInput
                    {
                        ProjectId = Project.Id,
                        LanguageCode = LanguageCode,
                        Version = Version
                    });


            if (projectParameters?.Parameters == null)
            {
                return;
            }

            var availableparameters = await _documentSectionRenderer.GetAvailableParametersAsync(Document.Content);

            DocumentPreferences = new DocumentParametersDto
            {
                Parameters = new List<DocumentParameterDto>()
            };

            if (availableparameters == null || !availableparameters.Any())
            {
                return;
            }

            foreach (var parameter in projectParameters.Parameters)
            {
                var availableParameter = availableparameters.GetOrDefault(parameter.Name);
                if (availableParameter != null)
                {
                    var newParameter = new DocumentParameterDto
                    {
                        Name = parameter.Name,
                        DisplayName = parameter.DisplayName,
                        Values = new Dictionary<string, string>()
                    };

                    foreach (var value in parameter.Values)
                    {
                        if (availableParameter.Contains(value.Key))
                        {
                            newParameter.Values.Add(value.Key, value.Value);
                        }
                    }

                    DocumentPreferences.Parameters.Add(newParameter);
                }
            }
        }
    }
}
