using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Octokit;
using Octokit.Internal;
using Volo.Abp.Domain.Services;
using Volo.Docs.Documents;
using Volo.Docs.GitHub.Projects;
using Volo.Docs.Projects;
using ProductHeaderValue = Octokit.ProductHeaderValue;

namespace Volo.Docs.GitHub.Documents
{
    public class GithubDocumentStore : DomainService, IDocumentStore
    {
        public const string Type = "GitHub";

        public Task<Document> FindDocument(Docs.Projects.Project project, string documentName, string version)
        {
            var rootUrl = project.GetGitHubUrl()
                .Replace("_version_/", version + "/")
                .Replace("www.", ""); //TODO: Can be a problem?

            var rawRootUrl = rootUrl
                .Replace("github.com", "raw.githubusercontent.com")
                .Replace("/tree/", "/"); //TODO: Replacing this can be a problem if I have a tree folder inside the repository

            var rawUrl = rawRootUrl + documentName;
            var editLink = rootUrl.Replace("/tree/", "/blob/") + documentName;
            var localDirectory = "";
            var fileName = documentName;

            if (documentName.Contains("/"))
            {
                localDirectory = documentName.Substring(0, documentName.LastIndexOf('/'));
                fileName = documentName.Substring(
                    documentName.LastIndexOf('/') + 1,
                    documentName.Length - documentName.LastIndexOf('/') - 1
                );
            }

            var token = project.GetGitHubAccessTokenOrNull();

            var document = new Document
            {
                Title = documentName,
                EditLink = editLink,
                RootUrl = rootUrl,
                RawRootUrl = rawRootUrl,
                Format = project.Format,
                LocalDirectory = localDirectory,
                FileName = fileName,
                Version = version,
                Content = DownloadWebContent(rawUrl, token)
            };

            return Task.FromResult(document);
        }

        private string DownloadWebContent(string rawUrl, string token)
        {
            try
            {
                using (var webClient = new WebClient())
                {
                    if (!token.IsNullOrWhiteSpace())
                    {
                        webClient.Headers.Add("Authorization", "token " + token);
                    }

                    return webClient.DownloadString(rawUrl);
                }
            }
            catch (Exception ex) //TODO: Only handle when document is really not available
            {
                Logger.LogWarning(ex.Message, ex);
                throw new DocumentNotFoundException(rawUrl);
            }
        }

        public async Task<List<VersionInfo>> GetVersions(Volo.Docs.Projects.Project project)
        {
            try
            {
                var token = project.GetGitHubAccessTokenOrNull();

                var gitHubClient = token.IsNullOrWhiteSpace()
                    ? new GitHubClient(new ProductHeaderValue("AbpWebSite"))
                    : new GitHubClient(new ProductHeaderValue("AbpWebSite"), new InMemoryCredentialStore(new Credentials(token)));

                var url = project.GetGitHubUrl();
                var releases = await gitHubClient.Repository.Release.GetAll(
                    GetGithubOrganizationNameFromUrl(url),
                    GetGithubRepositoryNameFromUrl(url)
                );

                return releases.OrderByDescending(r => r.PublishedAt).Select(r => new VersionInfo { Name = r.TagName, DisplayName = r.TagName }).ToList();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex);
                return new List<VersionInfo>();
            }
        }

        private static string GetGithubOrganizationNameFromUrl(string url)
        {
            try
            {
                var urlStartingAfterFirstSlash =
                    url.Substring(url.IndexOf("github.com/", StringComparison.OrdinalIgnoreCase) + "github.com/".Length);
                return urlStartingAfterFirstSlash.Substring(0, urlStartingAfterFirstSlash.IndexOf('/'));
            }
            catch (Exception)
            {
                throw new Exception($"Github url is not valid: {url}");
            }
        }

        private string GetGithubRepositoryNameFromUrl(string url)
        {
            try
            {
                var urlStartingAfterFirstSlash =
                    url.Substring(url.IndexOf("github.com/", StringComparison.OrdinalIgnoreCase) + "github.com/".Length);
                var urlStartingAfterSecondSlash =
                    urlStartingAfterFirstSlash.Substring(urlStartingAfterFirstSlash.IndexOf('/') + 1);
                return urlStartingAfterSecondSlash.Substring(0, urlStartingAfterSecondSlash.IndexOf('/'));
            }
            catch (Exception)
            {
                throw new Exception($"Github url is not valid: {url}");
            }
        }
    }
}