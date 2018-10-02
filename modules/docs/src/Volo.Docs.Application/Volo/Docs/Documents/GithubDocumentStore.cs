using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Octokit;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using ProductHeaderValue = Octokit.ProductHeaderValue;

namespace Volo.Docs.Documents
{
    public class GithubDocumentStore : DomainService, IDocumentStore
    {
        public const string Type = "Github"; //TODO: Convert to "github"

        public const int DocumentNotFoundExceptionCode = 20181001;

        public async Task<Document> FindDocumentByNameAsync(Dictionary<string, object> projectExtraProperties, string projectFormat, string documentName, string version)
        {
            var rootUrl = projectExtraProperties["GithubRootUrl"].ToString().Replace("_version_/", version + "/").Replace("www.", "");

            var token = projectExtraProperties["GithubAccessToken"]?.ToString();

            var rawRootUrl = rootUrl.Replace("github.com", token + "raw.githubusercontent.com").Replace("/tree/", "/");
            var rawUrl = rawRootUrl + documentName;
            var editLink = rootUrl.Replace("/tree/", "/blob/") + documentName;
            string localDirectory = "";
            string fileName = documentName;

            if (documentName.Contains("/"))
            {
                localDirectory = documentName.Substring(0, documentName.LastIndexOf('/'));
                fileName = documentName.Substring(documentName.LastIndexOf('/') + 1,
                    documentName.Length - documentName.LastIndexOf('/') - 1);
            }

            var document = new Document
            {
                Title = documentName,
                EditLink = editLink,
                RootUrl = rootUrl,
                RawRootUrl = rawRootUrl,
                Format = projectFormat,
                LocalDirectory = localDirectory,
                FileName = fileName,
                Version = version,
                SuccessfullyRetrieved = TryDownloadWebContent(rawUrl, out var content),
                Content = content
            };

            return await Task.FromResult(document);
        }

        private bool TryDownloadWebContent(string rawUrl, out string content)
        {
            using (var webClient = new WebClient())
            {
                try
                {
                    content = webClient.DownloadString(rawUrl);
                    return true;
                }
                catch (Exception ex)
                {
                    content = null;
                    Logger.LogError(ex, ex.Message);
                    return false;
                }
            }
        }

        public async Task<List<string>> GetVersions(Dictionary<string, object> projectExtraProperties, string documentName)
        {
            try
            {
                var gitHubClient = new GitHubClient(new ProductHeaderValue("AbpWebSite"));
                var url = projectExtraProperties["GithubRootUrl"].ToString();
                var releases = await gitHubClient.Repository.Release.GetAll(GetGithubOrganizationNameFromUrl(url),
                    GetGithubRepositoryNameFromUrl(url));

                return releases.OrderByDescending(r => r.PublishedAt).Select(r => r.TagName).ToList();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex);
                return new List<string>();
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