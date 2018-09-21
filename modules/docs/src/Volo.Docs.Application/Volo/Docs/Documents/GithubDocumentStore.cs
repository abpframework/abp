using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Octokit;
using Volo.Abp.Domain.Services;
using ProductHeaderValue = Octokit.ProductHeaderValue;
using Project = Volo.Docs.Projects.Project;

namespace Volo.Docs.Documents
{
    public class GithubDocumentStore : DomainService, IDocumentStore
    {
        public const string Type = "Github"; //TODO: Convert to "github"

        public async Task<Document> FindDocumentByNameAsync(Project project, string documentName, string version)
        {
            var rootUrl = project.ExtraProperties["GithubRootUrl"].ToString().Replace("_version_/", version + "/").Replace("www.", "");
            var token = project.ExtraProperties["GithubAccessToken"]?.ToString();

            var rawRootUrl = rootUrl.Replace("github.com", token + "raw.githubusercontent.com").Replace("/tree/", "/");
            var rawUrl = rawRootUrl + documentName;
            var editLink = rootUrl.Replace("/tree/", "/blob/") + documentName;
            string localDirectory = "";
            string fileName = documentName;

            if (documentName.Contains("/"))
            {
                localDirectory = documentName.Substring(0, documentName.LastIndexOf('/'));
                fileName = documentName.Substring(documentName.LastIndexOf('/') + 1, documentName.Length - documentName.LastIndexOf('/') - 1);
            }

            var content = DownloadWebContent(documentName, rawUrl);

            return await Task.FromResult(new Document
            {
                Title = documentName,
                Content = content,
                EditLink = editLink,
                RootUrl = rootUrl,
                RawRootUrl = rawRootUrl,
                Format = project.Format,
                LocalDirectory = localDirectory,
                FileName = fileName
            });
        }

        private string DownloadWebContent(string documentName, string rawUrl)
        {
            using (var webClient = new WebClient())
            {
                try
                {
                    return webClient.DownloadString(rawUrl);
                }
                catch (WebException ex)
                {
                    Logger.LogError(ex, ex.Message);

                    if (ex.Status == WebExceptionStatus.ProtocolError)
                    {
                        if (ex.Response != null && ex.Response is HttpWebResponse response)
                        {
                            if (response.StatusCode == HttpStatusCode.NotFound)
                            {
                                return $"The document {documentName} not found in this version!";
                            }
                        }
                    }

                    return "An error occured while getting the document " + documentName;
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, ex.Message);
                    return "An error occured while getting the document " + documentName;
                }
            }
        }

        public async Task<List<string>> GetVersions(Project project, string documentName)
        {
            var gitHubClient = new GitHubClient(new ProductHeaderValue("AbpWebSite"));
            var url = project.ExtraProperties["GithubRootUrl"].ToString();
            var releases = await gitHubClient.Repository.Release.GetAll(GetGithubOrganizationNameFromUrl(url), GetGithubRepositoryNameFromUrl(url));

            return releases.OrderByDescending(r => r.PublishedAt).Select(r => r.TagName).ToList();
        }

        private static string GetGithubOrganizationNameFromUrl(string url)
        {
            try
            {
                var urlStartingAfterFirstSlash = url.Substring(url.IndexOf("github.com/", StringComparison.Ordinal) + "github.com/".Length);
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
                var urlStartingAfterFirstSlash = url.Substring(url.IndexOf("github.com/", StringComparison.Ordinal) + "github.com/".Length);
                var urlStartingAfterSecondSlash = urlStartingAfterFirstSlash.Substring(urlStartingAfterFirstSlash.IndexOf('/') + 1);
                return urlStartingAfterSecondSlash.Substring(0, urlStartingAfterSecondSlash.IndexOf('/'));
            }
            catch (Exception)
            {
                throw new Exception($"Github url is not valid: {url}");
            }
        }

    }
}