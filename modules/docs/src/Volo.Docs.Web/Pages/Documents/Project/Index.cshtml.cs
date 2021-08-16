using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.Domain.Entities;
using Volo.Docs.Documents;
using Volo.Docs.HtmlConverting;
using Volo.Docs.Models;
using Volo.Docs.Projects;
using Volo.Docs.GitHub.Documents.Version;
using Volo.Docs.Localization;

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

        public bool ProjectFound { get; set; } = true;

        public bool LoadSuccess => DocumentFound && ProjectFound;

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

        public bool IsVersionPreview { get; set; }

        public string DocumentLanguageCode { get; set; }

        public DocumentParametersDto DocumentPreferences { get; set; }

        public DocumentRenderParameters UserPreferences { get; set; } = new DocumentRenderParameters();

        public List<string> AlternativeOptionLinkQueries { get; set; } = new List<string>();

        public bool FullSearchEnabled { get; set; }

        private const int MaxDescriptionMetaTagLength = 200;
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

            LocalizationResourceType = typeof(DocsResource);
        }

        public virtual async Task<IActionResult> OnGetAsync()
        {
            try
            {
                return await SetPageAsync();
            }
            catch (DocumentNotFoundException exception)
            {
                Logger.LogWarning(exception.Message);

                DocumentFound = false;
                Response.StatusCode = 404;
                return Page();
            }
        }

        private async Task<IActionResult> SetPageAsync()
        {
            DocumentsUrlPrefix = _uiOptions.RoutePrefix;
            ShowProjectsCombobox = _uiOptions.ShowProjectsCombobox;
            FullSearchEnabled = await _documentAppService.FullSearchEnabledAsync();

            try
            {
                await SetProjectAsync();
            }
            catch (EntityNotFoundException e)
            {
                Logger.LogWarning(e.Message);
                ProjectFound = false;
                Response.StatusCode = 404;
                return Page();
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
            var sb = new StringBuilder();

            var returnUrl = sb.Append(DocumentsUrlPrefix).Append(LanguageCode).Append("/").Append(ProjectName)
                .Append("/").Append(LatestVersionInfo.IsSelected ? DocsAppConsts.Latest : Version).Append("/").Append(DocumentName).ToString();

            sb.Clear();
            
            return Redirect(sb.Append("/Abp/Languages/Switch?culture=").Append(LanguageCode).Append("&uiCulture=")
                .Append(LanguageCode).Append("&returnUrl=").Append(returnUrl).ToString());
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

            var sb = new StringBuilder();
            
            ProjectSelectItems = projects.Items.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id != Project.Id ? sb.Append(DocumentsUrlPrefix).Append(LanguageCode).Append("/").Append(p.ShortName).Append("/").Append(DocsAppConsts.Latest).ToString() : null,
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
            var versions = output.Items.ToList()
                .Select(v => new VersionInfoViewModel(v.DisplayName, v.Name))
                .ToList();

            if (versions.Any())
            {
                LatestVersionInfo = GetLatestVersionInfo(versions);

                SetLatestVersionBranchName(versions);

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
                        IsVersionPreview = versionFromUrl.IsPreview;
                        Version = versionFromUrl.Version;
                    }
                    else
                    {
                        LatestVersionInfo.IsSelected = true;
                        Version = LatestVersionInfo.Version;
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

        private VersionInfoViewModel GetLatestVersionInfo(List<VersionInfoViewModel> versions)
        {
            if (Project.ExtraProperties.ContainsKey("GithubVersionProviderSource")
                && (GithubVersionProviderSource) (long) Project.ExtraProperties["GithubVersionProviderSource"] == GithubVersionProviderSource.Branches)
            {
                var LatestVersionBranchNameWithoutPrefix = RemoveVersionPrefix(Project.LatestVersionBranchName);

                foreach (var version in versions)
                {
                    if (version.Version == LatestVersionBranchNameWithoutPrefix)
                    {
                        return version;
                    }

                    version.DisplayText = $"{version.DisplayText} ({L["Preview"].Value})";
                    version.IsPreview = true;

                }
            }

            return versions.FirstOrDefault(v => !SemanticVersionHelper.IsPreRelease(v.Version)) ?? versions.First();
        }

        private string RemoveVersionPrefix(string version)
        {
            if (!Project.ExtraProperties.ContainsKey("VersionBranchPrefix"))
            {
                return version;
            }

            var prefix = Project.ExtraProperties["VersionBranchPrefix"].ToString();

            if (string.IsNullOrWhiteSpace(version) || !version.StartsWith(prefix) || version.Length <= prefix.Length)
            {
                return version;
            }

            return version.Substring(prefix.Length);
        }

        private void SetLatestVersionBranchName(List<VersionInfoViewModel> versions)
        {
            if (!Project.ExtraProperties.ContainsKey("GithubVersionProviderSource")
                || (GithubVersionProviderSource) (long) Project.ExtraProperties["GithubVersionProviderSource"] == GithubVersionProviderSource.Releases)
            {
                versions.First(v=> !SemanticVersionHelper.IsPreRelease(v.Version)).Version = Project.LatestVersionBranchName;
            }
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

            var linkStringBuilder = new StringBuilder();
            linkStringBuilder.Append(DocumentsUrlPrefix).Append(LanguageCode).Append("/").Append(ProjectName).Append("/").Append(version);

            if (documentName != null)
            {
                linkStringBuilder.Append("/").Append(DocumentName);
            }

            return linkStringBuilder.ToString();
        }

        public string GetSpecificVersionOrLatest()
        {
            if (Document?.Version == null)
            {
                return DocsAppConsts.Latest;
            }

            return RemoveVersionPrefix(Document.Version) == LatestVersionInfo.Version ?
                DocsAppConsts.Latest :
                RemoveVersionPrefix(Document.Version);
        }

        private async Task SetDocumentAsync()
        {
            var sb = new StringBuilder();
            DocumentNameWithExtension = sb.Append(DocumentName).Append(".").Append(Project.Format).ToString();

            try
            {
                Document = await GetSpecificDocumentOrDefaultAsync(LanguageCode);
                DocumentLanguageCode = LanguageCode;
            }
            catch (DocumentNotFoundException)
            {
                if (LanguageCode != DefaultLanguageCode)
                {
                    Document = await GetSpecificDocumentOrDefaultAsync(DefaultLanguageCode);

                    DocumentLanguageCode = DefaultLanguageCode;
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

            var sb = new StringBuilder();
            
            foreach (var language in LanguageConfig.Languages)
            {
                LanguageSelectListItems.Add(
                    new SelectListItem(
                        language.DisplayName,
                        sb.Append(DocumentsUrlPrefix).Append(language.Code).Append("/").Append(Project.ShortName).Append("/").Append(LatestVersionInfo.IsSelected ? DocsAppConsts.Latest : Version).Append("/").Append(DocumentName).ToString(),
                        language.Code == LanguageCode
                        )
                    );

                sb.Clear();
            }
        }

        private async Task ConvertDocumentContentToHtmlAsync()
        {
            if (_uiOptions.SectionRendering)
            {
                await SetDocumentPreferencesAsync();
                SetAlternativeOptionLinksAsync();
                SetUserPreferences();

                var partialTemplates = await GetDocumentPartialTemplatesAsync();

                Document.Content = await _documentSectionRenderer.RenderAsync(Document.Content, UserPreferences, partialTemplates);
            }

            var converter = _documentToHtmlConverterFactory.Create(Document.Format ?? Project.Format);
            var content = converter.Convert(Project, Document, GetSpecificVersionOrLatest(), LanguageCode);

            content = HtmlNormalizer.ReplaceImageSources(
                content,
                Document.RawRootUrl,
                Document.LocalDirectory
            );

            content = HtmlNormalizer.WrapImagesWithinAnchors(content);

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
                    LanguageCode = DocumentLanguageCode,
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
            UserPreferences.Add("Document_Language_Code", DocumentLanguageCode);
            UserPreferences.Add("Document_Version", Version);
            UserPreferences.Add("Release_Status", IsVersionPreview ? "preview" : "stable");

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

                var values = DocumentPreferences?.Parameters?.FirstOrDefault(p => p.Name == key)?.Values;

                if (values == null)
                {
                    continue;
                }

                if (!values.Any(v => v.Key == value))
                {
                    var defaultValue = values.FirstOrDefault();
                    UserPreferences.Add(key, defaultValue.Key);
                    UserPreferences.Add(key + "_Value", defaultValue.Value);

                    continue;
                }

                UserPreferences.Add(key, value);
                UserPreferences.Add(key + "_Value", values.FirstOrDefault(v => v.Key == value).Value);
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

        private async Task<DocumentWithDetailsDto> GetSpecificDocumentOrDefaultAsync(string languageCode)
        {
            if (DocumentName.IsNullOrWhiteSpace())
            {
                return await _documentAppService.GetDefaultAsync(
                    new GetDefaultDocumentInput
                    {
                        ProjectId = Project.Id,
                        LanguageCode = languageCode,
                        Version = Version
                    }
                );
            }
            
            
            return await _documentAppService.GetAsync(
                new GetDocumentInput
                {
                    ProjectId = Project.Id,
                    Name = DocumentNameWithExtension,
                    LanguageCode = languageCode,
                    Version = Version
                }
            );
        }

        private async Task SetDocumentPreferencesAsync()
        {
            var projectParameters = await _documentAppService.GetParametersAsync(
                    new GetParametersDocumentInput
                    {
                        ProjectId = Project.Id,
                        LanguageCode = DocumentLanguageCode,
                        Version = Version
                    });


            if (projectParameters?.Parameters == null)
            {
                return;
            }

            var availableParameters = await _documentSectionRenderer.GetAvailableParametersAsync(Document.Content);

            DocumentPreferences = new DocumentParametersDto
            {
                Parameters = new List<DocumentParameterDto>()
            };

            if (availableParameters == null || !availableParameters.Any())
            {
                return;
            }

            foreach (var parameter in projectParameters.Parameters)
            {
                var availableParameter = availableParameters.GetOrDefault(parameter.Name);
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

        private void SetAlternativeOptionLinksAsync()
        {
            if (!DocumentPreferences?.Parameters?.Any() ?? true)
            {
                return;
            }

            AlternativeOptionLinkQueries = CollectAlternativeOptionLinksRecursively();
        }

        private List<string> CollectAlternativeOptionLinksRecursively(int index = 0)
        {
            if (index >= DocumentPreferences.Parameters.Count)
            {
                return new List<string>();
            }

            var option = DocumentPreferences.Parameters[index];
            var queries = new List<string>();

            foreach (var key in option.Values.Keys)
            {
                var linkQuery = new StringBuilder($"{option.Name}={key}");

                var restOfQueries = CollectAlternativeOptionLinksRecursively(index + 1);

                if (restOfQueries.Any())
                {
                    foreach (var restOfQuery in restOfQueries)
                    {
                        queries.Add($"{linkQuery}&{restOfQuery}");
                    }
                }
                else
                {
                    queries.Add($"{linkQuery}");
                }
            }

            return queries;
        }

        public string GetDescription()
        {
            if (Document == null || Document.Content.IsNullOrWhiteSpace())
            {
                return null;
            }

            var firstParagraph = new Regex(@"<p>(.*?)</p>", RegexOptions.IgnoreCase);
            var match = firstParagraph.Match(Document.Content);
            if (!match.Success)
            {
                return null;
            }

            var description = HttpUtility.HtmlDecode(match.Value);

            var htmlTagReplacer = new Regex(@"<[^>]*>", RegexOptions.IgnoreCase);
            description = htmlTagReplacer.Replace(description, m => string.Empty);

            return description.Truncate(MaxDescriptionMetaTagLength);
        }
    }
}