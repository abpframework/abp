using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.Domain.Services;
using Volo.Docs.Documents;
using Volo.Docs.GitHub.Projects;
using Volo.Docs.Projects;
using Octokit;
using Volo.Abp;
using Volo.Docs.GitHub.Documents.Version;
using Volo.Extensions;
using Project = Volo.Docs.Projects.Project;

namespace Volo.Docs.GitHub.Documents
{
    //TODO: Needs more refactoring

    public class GithubDocumentSource : DomainService, IDocumentSource
    {
        public const string Type = "GitHub";

        private readonly IGithubRepositoryManager _githubRepositoryManager;
        private readonly IGithubPatchAnalyzer _githubPatchAnalyzer;
        private readonly IVersionHelper _versionHelper;

        public GithubDocumentSource(IGithubRepositoryManager githubRepositoryManager, IGithubPatchAnalyzer githubPatchAnalyzer, IVersionHelper versionHelper)
        {
            _githubRepositoryManager = githubRepositoryManager;
            _githubPatchAnalyzer = githubPatchAnalyzer;
            _versionHelper = versionHelper;
        }

        public virtual async Task<Document> GetDocumentAsync(Project project, string documentName, string languageCode, string version, DateTime? lastKnownSignificantUpdateTime = null)
        {
            var token = project.GetGitHubAccessTokenOrNull();
            var rootUrl = project.GetGitHubUrl(version);
            var userAgent = project.GetGithubUserAgentOrNull();
            var rawRootUrl = CalculateRawRootUrlWithLanguageCode(rootUrl, languageCode);
            var rawDocumentUrl = rawRootUrl + documentName;
            var isNavigationDocument = documentName == project.NavigationDocumentName;
            var isParameterDocument = documentName == project.ParametersDocumentName;
            var editLink = rootUrl.ReplaceFirst("/tree/", "/blob/").EnsureEndsWith('/') + languageCode + "/" + documentName;
            var localDirectory = "";
            var fileName = documentName;

            if (documentName.Contains("/"))
            {
                localDirectory = documentName.Substring(0, documentName.LastIndexOf('/'));
                fileName = documentName.Substring(documentName.LastIndexOf('/') + 1);
            }

            var content = await DownloadWebContentAsStringAsync(rawDocumentUrl, token, userAgent);
            var commits = await GetGitHubCommitsOrNull(project, documentName, languageCode, version);

            var documentCreationTime = GetFirstCommitDate(commits);
            var lastUpdateTime = GetLastCommitDate(commits);
            var lastSignificantUpdateTime = await GetLastKnownSignificantUpdateTime(project, documentName, languageCode, version, lastKnownSignificantUpdateTime, isNavigationDocument, isParameterDocument, commits, documentCreationTime);

            var document = new Document
            (
                GuidGenerator.Create(),
                project.Id,
                documentName,
                version,
                languageCode,
                fileName,
                content,
                project.Format,
                editLink,
                rootUrl,
                rawRootUrl,
                localDirectory,
                documentCreationTime,
                lastUpdateTime,
                DateTime.Now,
                lastSignificantUpdateTime
            );

            if (isNavigationDocument || isParameterDocument)
            {
                return document;
            }

            var authors = GetAuthors(commits);

            document.RemoveAllContributors();
            foreach (var author in authors)
            {
                document.AddContributor(author.Login, author.HtmlUrl, author.AvatarUrl, author.CommitCount);
            }

            return document;
        }

        private async Task<DateTime?> GetLastKnownSignificantUpdateTime(
            Project project,
            string documentName,
            string languageCode,
            string version,
            DateTime? lastKnownSignificantUpdateTime,
            bool isNavigationDocument,
            bool isParameterDocument,
            IReadOnlyList<GitHubCommit> commits,
            DateTime documentCreationTime)
        {
            return !isNavigationDocument && !isParameterDocument && version == project.LatestVersionBranchName
                ? await GetLastSignificantUpdateTime(
                      commits,
                      project,
                      project.GetGitHubInnerUrl(languageCode, documentName),
                      lastKnownSignificantUpdateTime,
                      documentCreationTime
                  ) ?? lastKnownSignificantUpdateTime
                : null;
        }

        private static List<DocumentAuthor> GetAuthors(IReadOnlyList<GitHubCommit> commits)
        {
            if (commits == null || !commits.Any())
            {
                return new List<DocumentAuthor>();
            }

            var authorsOrderedAndGrouped = commits
                .Where(x => x.Author != null)
                .Select(x => x.Author)
                .GroupBy(x => x.Id)
                .OrderByDescending(x => x.Count());

            var documentAuthors = new List<DocumentAuthor>();

            foreach (var authorGroup in authorsOrderedAndGrouped)
            {
                var author =  authorGroup.FirstOrDefault();
                var documentAuthor = new DocumentAuthor
                {
                    CommitCount = authorGroup.Count(),
                    AvatarUrl = author.AvatarUrl,
                    HtmlUrl = author.HtmlUrl,
                    Login = author.Login
                };
                documentAuthors.Add(documentAuthor);
            }

            return documentAuthors;
        }

        private static DateTime GetLastCommitDate(IReadOnlyList<GitHubCommit> commits)
        {
            return GetCommitDate(commits, false);
        }

        private static DateTime GetFirstCommitDate(IReadOnlyList<GitHubCommit> commits)
        {
            return GetCommitDate(commits, true);
        }

        private static DateTime GetCommitDate(IReadOnlyList<GitHubCommit> commits, bool isFirstCommit)
        {
            if (commits == null)
            {
                return DateTime.MinValue;
            }

            var gitHubCommit = isFirstCommit ?
                commits.LastOrDefault() : //first commit
                commits.FirstOrDefault(); //last commit

            if (gitHubCommit == null)
            {
                return DateTime.MinValue;
            }

            if (gitHubCommit.Commit == null)
            {
                return DateTime.MinValue;
            }

            if (gitHubCommit.Commit.Author == null)
            {
                return DateTime.MinValue;
            }

            return gitHubCommit.Commit.Author.Date.DateTime;
        }

        private async Task<IReadOnlyList<GitHubCommit>> GetGitHubCommitsOrNull(Project project, string documentName, string languageCode, string version)
        {
            /*
            * Getting file commits usually throws "Resource temporarily unavailable" or "Network is unreachable"
            * This is a trival information and running this inside try-catch is safer.
            */

            try
            {
                return await GetFileCommitsAsync(project, version, project.GetGitHubInnerUrl(languageCode, documentName));
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
                return null;
            }
        }

        private async Task<DateTime?> GetLastSignificantUpdateTime(
            IReadOnlyList<GitHubCommit> commits,
            Project project,
            string fileName,
            DateTime? lastKnownSignificantUpdateTime,
            DateTime documentCreationTime)
        {
            if (commits == null || !commits.Any())
            {
                return null;
            }

            var fileCommitsAfterCreation = commits.Take(commits.Count - 1);

            var commitsToEvaluate = (lastKnownSignificantUpdateTime != null
                ? fileCommitsAfterCreation.Where(c => c.Commit.Author.Date.DateTime > lastKnownSignificantUpdateTime)
                : fileCommitsAfterCreation).Where(c => c.Commit.Author.Date.DateTime > DateTime.Now.AddDays(-14));

            foreach (var gitHubCommit in commitsToEvaluate)
            {
                var fullCommit = await _githubRepositoryManager.GetSingleCommitsAsync(
                    GetOwnerNameFromUrl(project.GetGitHubUrl()),
                    GetRepositoryNameFromUrl(project.GetGitHubUrl()),
                    gitHubCommit.Sha,
                    project.GetGitHubAccessTokenOrNull());

                if (_githubPatchAnalyzer.HasPatchSignificantChanges(fullCommit.Files.First(f => f.Filename == fileName).Patch))
                {
                    return gitHubCommit.Commit.Author.Date.DateTime;
                }
            }

            return null;
        }

        public async Task<List<VersionInfo>> GetVersionsAsync(Project project)
        {
            var url = project.GetGitHubUrl();
            var ownerName = GetOwnerNameFromUrl(url);
            var repositoryName = GetRepositoryNameFromUrl(url);
            var githubVersionProviderSource = GetGithubVersionProviderSource(project);

            List<VersionInfo> versions;
            try
            {
                versions = (await _githubRepositoryManager.GetVersionsAsync(ownerName, repositoryName, project.GetGitHubAccessTokenOrNull(), githubVersionProviderSource))
                    .Select(r => new VersionInfo
                    {
                        Name = r.Name,
                        DisplayName = r.Name
                    }).ToList();
            }
            catch (Exception ex)
            {
                //TODO: It may not be a good idea to hide the error!
                Logger.LogError(ex.Message, ex);
                versions = new List<VersionInfo>();
            }

            if (githubVersionProviderSource == GithubVersionProviderSource.Branches && project.ExtraProperties.ContainsKey("VersionBranchPrefix"))
            {
                var prefix = (string) project.ExtraProperties["VersionBranchPrefix"];

                if (!string.IsNullOrEmpty(prefix))
                {
                    versions = versions.Where(v => v.Name.StartsWith(prefix)).ToList();
                    foreach (var v in versions)
                    {
                        v.Name = v.Name.Substring(prefix.Length);
                        v.DisplayName = v.DisplayName.Substring(prefix.Length);
                    }
                }

                versions = _versionHelper.OrderByDescending(versions);
            }

            if(githubVersionProviderSource == GithubVersionProviderSource.Releases)
            {
                if (!versions.Any() && !string.IsNullOrEmpty(project.LatestVersionBranchName))
                {
                    versions.Add(new VersionInfo { DisplayName = "1.0.0", Name = project.LatestVersionBranchName });
                }
                else
                {
                    versions = _versionHelper.OrderByDescending(versions);
                }
            }

            return versions;
        }

        private GithubVersionProviderSource GetGithubVersionProviderSource(Project project)
        {
            return project.ExtraProperties.ContainsKey("GithubVersionProviderSource")
                ? (GithubVersionProviderSource) (long) project.ExtraProperties["GithubVersionProviderSource"]
                : GithubVersionProviderSource.Releases;
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

            var url = CalculateRawRootUrl(rootUrl) + DocsDomainConsts.LanguageConfigFileName;

            var configAsJson = await DownloadWebContentAsStringAsync(url, token, userAgent);

            if (!DocsJsonSerializerHelper.TryDeserialize<LanguageConfig>(configAsJson, out var languageConfig))
            {
                throw new UserFriendlyException($"Cannot validate language config file '{DocsDomainConsts.LanguageConfigFileName}' for the project {project.Name} - v{version}.");
            }

            return languageConfig;
        }

        private async Task<IReadOnlyList<GitHubCommit>> GetFileCommitsAsync(Project project, string version, string filename)
        {
            var url = project.GetGitHubUrl();
            var ownerName = GetOwnerNameFromUrl(url);
            var repositoryName = GetRepositoryNameFromUrl(url);

            return await _githubRepositoryManager.GetFileCommitsAsync(
                ownerName,
                repositoryName,
                version,
                filename,
                project.GetGitHubAccessTokenOrNull()
            );
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
                throw new Exception($"GitHub url is not valid: {url}");
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
