using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.Domain.Services;
using Volo.Docs.Documents;
using Volo.Docs.GitHub.Projects;
using Volo.Docs.Projects;
using Newtonsoft.Json.Linq;
using Octokit;
using ProductHeaderValue = Octokit.ProductHeaderValue;
using Project = Volo.Docs.Projects.Project;

namespace Volo.Docs.GitHub.Documents
{
    //TODO: Needs more refactoring

    public class GithubDocumentStore : DomainService, IDocumentStore
    {
        public const string Type = "GitHub";

        private readonly IGithubRepositoryManager _githubRepositoryManager;

        public GithubDocumentStore(IGithubRepositoryManager githubRepositoryManager)
        {
            _githubRepositoryManager = githubRepositoryManager;
        }
        
        public virtual async Task<Document> GetDocumentAsync(Project project, string documentName, string version)
        {
            var token = project.GetGitHubAccessTokenOrNull();
            var rootUrl = project.GetGitHubUrl(version);
            var rawRootUrl = CalculateRawRootUrl(rootUrl);
            var rawDocumentUrl = rawRootUrl + documentName;
            var commitHistoryUrl = project.GetGitHubUrlForCommitHistory() + documentName;
            var userAgent = project.GetGithubUserAgentOrNull();
            var isNavigationDocument = documentName == project.NavigationDocumentName;
            var editLink = rootUrl.ReplaceFirst("/tree/", "/blob/") + documentName;
            var localDirectory = "";
            var fileName = documentName;

            if (documentName.Contains("/"))
            {
                localDirectory = documentName.Substring(0, documentName.LastIndexOf('/'));
                fileName = documentName.Substring(documentName.LastIndexOf('/') + 1);
            }

            return new Document
            {
                Title = documentName,
                EditLink = editLink,
                RootUrl = rootUrl,
                RawRootUrl = rawRootUrl,
                Format = project.Format,
                LocalDirectory = localDirectory,
                FileName = fileName,
                Contributors = new List<DocumentContributor>(),
                //Contributors = !isNavigationDocument ? await GetContributors(commitHistoryUrl, token, userAgent): new List<DocumentContributor>(),
                Version = version,
                Content = await DownloadWebContentAsStringAsync(rawDocumentUrl, token, userAgent)
            };
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

        public async Task<DocumentResource> GetResource(Project project, string resourceName, string version)
        {
            var rawRootUrl = CalculateRawRootUrl(project.GetGitHubUrl(version));
            var content = await DownloadWebContentAsByteArrayAsync(
                rawRootUrl + resourceName,
                project.GetGitHubAccessTokenOrNull(),
                project.GetGithubUserAgentOrNull()
            );

            return new DocumentResource(content);
        }

        private async Task<IReadOnlyList<Release>> GetReleasesAsync(Project project)
        {
            var url = project.GetGitHubUrl();
            var ownerName = GetOwnerNameFromUrl(url);
            var repositoryName = GetRepositoryNameFromUrl(url);
            return await _githubRepositoryManager.GetReleasesAsync(ownerName, repositoryName, project.GetGitHubAccessTokenOrNull());
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

        private async Task<List<DocumentContributor>> GetContributors(string url, string token, string userAgent)
        {
            var contributors = new List<DocumentContributor>();

            try
            {
                var commitsJsonAsString = await DownloadWebContentAsStringAsync(url, token, userAgent);

                var commits = JArray.Parse(commitsJsonAsString);

                foreach (var commit in commits)
                {
                    var author = commit["author"];

                    contributors.Add(new DocumentContributor
                    {
                        Username = (string)author["login"],
                        UserProfileUrl = (string)author["html_url"],
                        AvatarUrl = (string)author["avatar_url"]
                    });
                }

                contributors = contributors.GroupBy(c => c.Username).OrderByDescending(c=>c.Count())
                    .Select( c => c.FirstOrDefault()).ToList();
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex.Message);
            }
            
            return contributors;
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
