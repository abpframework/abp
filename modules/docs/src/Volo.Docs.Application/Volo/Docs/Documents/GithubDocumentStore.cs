using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Octokit;
using Volo.Abp.DependencyInjection;
using ProductHeaderValue = Octokit.ProductHeaderValue;
using Project = Volo.Docs.Projects.Project;

namespace Volo.Docs.Documents
{
    public class GithubDocumentStore : IDocumentStore, ITransientDependency
    {
        public const string Type = "Github";

        public async Task<Document> FindDocumentByNameAsync(Project project, string documentName, string version)
        {
            var rootUrl = project.ExtraProperties["GithubRootUrl"].ToString().Replace("_version_/", version + "/").Replace("www.","");
            var token = project.ExtraProperties["GithubAccessToken"]?.ToString();

            var rawRootUrl = rootUrl.Replace("github.com", token + "raw.githubusercontent.com").Replace("/tree/", "/");
            var rawUrl = rawRootUrl + $"{documentName}.md";
            var editLink = rootUrl.Replace("/tree/", "/blob/") + $"{documentName}.md";

            using (var webClient = new WebClient())
            {
                string content;

                try
                {
                    content = webClient.DownloadString(rawUrl);
                }
                catch (Exception)
                {
                    content = "The Document doesn't exist.";
                }

                return new Document
                {
                    Title = documentName,
                    Content = content,
                    EditLink = editLink,
                    RootUrl = rootUrl,
                    RawRootUrl = rawRootUrl,
                    Format = project.Format
                };
            }
        }

        public async Task<List<string>> GetVersions(Project project, string documentName)
        {
            var gitHubClient = new GitHubClient(new ProductHeaderValue("AbpWebSite"));
            var url = project.ExtraProperties["GithubRootUrl"].ToString();
            var releases = await gitHubClient.Repository.Release.GetAll(GetGithubOrganizationNameFromUrl(url), GetGithubRepositoryNameFromUrl(url));
            
            return releases.OrderByDescending(r => r.PublishedAt).Select(r => r.TagName).ToList();
        }

        private string GetGithubOrganizationNameFromUrl(string url)
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