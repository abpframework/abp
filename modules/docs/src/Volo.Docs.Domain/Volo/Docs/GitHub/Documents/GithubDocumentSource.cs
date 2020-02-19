using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Volo.Abp.Domain.Services;
using Volo.Docs.Documents;
using Volo.Docs.GitHub.Projects;
using Volo.Docs.Projects;
using Newtonsoft.Json.Linq;
using Octokit;
using Project = Volo.Docs.Projects.Project;

namespace Volo.Docs.GitHub.Documents
{
    //TODO: Needs more refactoring

    public class GithubDocumentSource : DomainService, IDocumentSource
    {
        public const string Type = "GitHub";

        private readonly IGithubRepositoryManager _githubRepositoryManager;

        public GithubDocumentSource(IGithubRepositoryManager githubRepositoryManager)
        {
            _githubRepositoryManager = githubRepositoryManager;
        }
        
        public virtual async Task<Document> GetDocumentAsync(Project project, string documentName, string languageCode, string version)
        {
            var token = project.GetGitHubAccessTokenOrNull();
            var rootUrl = project.GetGitHubUrl(version);
            var userAgent = project.GetGithubUserAgentOrNull();
            var rawRootUrl = CalculateRawRootUrlWithLanguageCode(rootUrl, languageCode);
            var rawDocumentUrl = rawRootUrl + documentName;
            var isNavigationDocument = documentName == project.NavigationDocumentName;
            var isParameterDocument = documentName == project.ParametersDocumentName;
            var editLink = rootUrl.ReplaceFirst("/tree/", "/blob/") + languageCode + "/" + documentName;
            var localDirectory = "";
            var fileName = documentName;

            if (documentName.Contains("/"))
            {
                localDirectory = documentName.Substring(0, documentName.LastIndexOf('/'));
                fileName = documentName.Substring(documentName.LastIndexOf('/') + 1);
            }

            var fileCommits = await GetFileCommitsAsync(project, version, $"docs/{languageCode}/{documentName}");

            var document=  new Document(GuidGenerator.Create(), 
                project.Id, 
                documentName, 
                version, 
                languageCode,
                fileName, 
                await DownloadWebContentAsStringAsync(rawDocumentUrl, token, userAgent),
                project.Format, 
                editLink, 
                rootUrl,
                rawRootUrl, 
                localDirectory,
                fileCommits.LastOrDefault()?.Commit.Author.Date.DateTime ?? DateTime.MinValue,
                fileCommits.FirstOrDefault()?.Commit.Author.Date.DateTime ?? DateTime.MinValue,
                DateTime.Now);

            var authors =  fileCommits
                .Where(x => x.Author != null)
                .Select(x => x.Author)
                .GroupBy(x => x.Id)
                .OrderByDescending(x => x.Count())
                .Select(x => x.FirstOrDefault()).ToList();

            if (!isNavigationDocument && !isParameterDocument)
            {
                foreach (var author in authors)
                {
                    document.AddContributor(author.Login, author.HtmlUrl, author.AvatarUrl);
                }
            }

            return document;
        }

        public async Task<List<VersionInfo>> GetVersionsAsync(Project project)
        {
            List<VersionInfo> versions;
            try
            {
                versions = (await GetReleasesAsync(project))
                    .OrderByDescending(r => r.PublishedAt)
                    .Select(r => new VersionInfo
                    {
                        Name = r.TagName,
                        DisplayName = r.TagName
                    }).ToList();
            }
            catch (Exception ex)
            {
                //TODO: It may not be a good idea to hide the error!
                Logger.LogError(ex.Message, ex);
                versions = new List<VersionInfo>();
            }

            if (!versions.Any() && !string.IsNullOrEmpty(project.LatestVersionBranchName))
            {
                versions.Add(new VersionInfo { DisplayName = "1.0.0", Name = project.LatestVersionBranchName });
            }

            return versions;
        }

        public async Task<DocumentResource> GetResource(Project project, string resourceName, string languageCode, string version)
        {
            var rawRootUrl = CalculateRawRootUrlWithLanguageCode(project.GetGitHubUrl(version), languageCode);
            var content = await DownloadWebContentAsByteArrayAsync(
                rawRootUrl + resourceName,
                project.GetGitHubAccessTokenOrNull(),
                project.GetGithubUserAgentOrNull()
            );

            return new DocumentResource(content);
        }

        public async Task<LanguageConfig> GetLanguageListAsync(Project project, string version)
        {
            var token = project.GetGitHubAccessTokenOrNull();
            var rootUrl = project.GetGitHubUrl(version);
            var userAgent = project.GetGithubUserAgentOrNull();

            var url = CalculateRawRootUrl(rootUrl) + "docs-langs.json";

            var configAsJson = await DownloadWebContentAsStringAsync(url, token, userAgent);

            return JsonConvert.DeserializeObject<LanguageConfig>(configAsJson);
        }

        private async Task<IReadOnlyList<Release>> GetReleasesAsync(Project project)
        {
            var url = project.GetGitHubUrl();
            var ownerName = GetOwnerNameFromUrl(url);
            var repositoryName = GetRepositoryNameFromUrl(url);
            return await _githubRepositoryManager.GetReleasesAsync(ownerName, repositoryName, project.GetGitHubAccessTokenOrNull());
        }

        private async Task<IReadOnlyList<GitHubCommit>> GetFileCommitsAsync(Project project, string version, string filename)
        {
            var url = project.GetGitHubUrl();
            var ownerName = GetOwnerNameFromUrl(url);
            var repositoryName = GetRepositoryNameFromUrl(url);
            return await _githubRepositoryManager.GetFileCommitsAsync(ownerName, repositoryName,
                version, filename, project.GetGitHubAccessTokenOrNull());
        }

        protected virtual string GetOwnerNameFromUrl(string url)
        {
            try
            {
                var urlStartingAfterFirstSlash = url.Substring(url.IndexOf("github.com/", StringComparison.OrdinalIgnoreCase) + "github.com/".Length);
                return urlStartingAfterFirstSlash.Substring(0, urlStartingAfterFirstSlash.IndexOf('/'));
            }
            catch (Exception)
            {
                throw new Exception($"Github url is not valid: {url}");
            }
        }

        protected virtual string GetRepositoryNameFromUrl(string url)
        {
            try
            {
                var urlStartingAfterFirstSlash = url.Substring(url.IndexOf("github.com/", StringComparison.OrdinalIgnoreCase) + "github.com/".Length);
                var urlStartingAfterSecondSlash = urlStartingAfterFirstSlash.Substring(urlStartingAfterFirstSlash.IndexOf('/') + 1);
                return urlStartingAfterSecondSlash.Substring(0, urlStartingAfterSecondSlash.IndexOf('/'));
            }
            catch (Exception)
            {
                throw new Exception($"Github url is not valid: {url}");
            }
        }

        private async Task<string> DownloadWebContentAsStringAsync(string rawUrl, string token, string userAgent)
        {
            try
            {
                Logger.LogInformation("Downloading content from Github (DownloadWebContentAsStringAsync): " + rawUrl);

                return await _githubRepositoryManager.GetFileRawStringContentAsync(rawUrl, token, userAgent);
            }
            catch (Exception ex)
            {
                //TODO: Only handle when document is really not available
                Logger.LogWarning(ex.Message, ex);
                throw new DocumentNotFoundException(rawUrl);
            }
        }

        private async Task<byte[]> DownloadWebContentAsByteArrayAsync(string rawUrl, string token, string userAgent)
        {
            try
            {
                Logger.LogInformation("Downloading content from Github (DownloadWebContentAsByteArrayAsync): " + rawUrl);

                return await _githubRepositoryManager.GetFileRawByteArrayContentAsync(rawUrl, token, userAgent);
            }
            catch (Exception ex)
            {
                //TODO: Only handle when resource is really not available
                Logger.LogWarning(ex.Message, ex);
                throw new ResourceNotFoundException(rawUrl);
            }
        }

        private static string CalculateRawRootUrlWithLanguageCode(string rootUrl, string languageCode)
        {
            return (rootUrl
                .Replace("github.com", "raw.githubusercontent.com")
                .ReplaceFirst("/tree/", "/")
                .EnsureEndsWith('/')
                + languageCode
                ).EnsureEndsWith('/');
        }

        private static string CalculateRawRootUrl(string rootUrl)
        {
            return rootUrl
                .Replace("github.com", "raw.githubusercontent.com")
                .ReplaceFirst("/tree/", "/")
                .EnsureEndsWith('/');
        }
    }
}
