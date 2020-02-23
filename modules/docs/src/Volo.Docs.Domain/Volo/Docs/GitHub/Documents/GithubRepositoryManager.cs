using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Octokit;
using Octokit.Internal;
using ProductHeaderValue = Octokit.ProductHeaderValue;

namespace Volo.Docs.GitHub.Documents
{
    public class GithubRepositoryManager : IGithubRepositoryManager
    {
        public const string HttpClientName = "GithubRepositoryManagerHttpClientName";

        private readonly IHttpClientFactory _clientFactory;

        public GithubRepositoryManager(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<string> GetFileRawStringContentAsync(string rawUrl, string token, string userAgent)
        {
            var httpClient = _clientFactory.CreateClient(HttpClientName);
            if (!token.IsNullOrWhiteSpace())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);
            }

            if (!userAgent.IsNullOrWhiteSpace())
            {
                httpClient.DefaultRequestHeaders.Add("User-Agent", userAgent);
            }

            return await httpClient.GetStringAsync(new Uri(rawUrl));
        }

        public async Task<byte[]> GetFileRawByteArrayContentAsync(string rawUrl, string token, string userAgent)
        {
            var httpClient = _clientFactory.CreateClient(HttpClientName);
            if (!token.IsNullOrWhiteSpace())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);
            }

            if (!userAgent.IsNullOrWhiteSpace())
            {
                httpClient.DefaultRequestHeaders.Add("User-Agent", userAgent);
            }

            return await httpClient.GetByteArrayAsync(new Uri(rawUrl));
        }

        public async Task<IReadOnlyList<Release>> GetReleasesAsync(string name, string repositoryName, string token)
        {
            var client = token.IsNullOrWhiteSpace()
                ? new GitHubClient(new ProductHeaderValue(name))
                : new GitHubClient(new ProductHeaderValue(name), new InMemoryCredentialStore(new Credentials(token)));

            return (await client
                .Repository
                .Release
                .GetAll(name, repositoryName)).ToList();
        }

        public async Task<IReadOnlyList<GitHubCommit>> GetFileCommitsAsync(string name, string repositoryName, string version, string filename, string token)
        {
            var client = token.IsNullOrWhiteSpace()
                ? new GitHubClient(new ProductHeaderValue(name))
                : new GitHubClient(new ProductHeaderValue(name), new InMemoryCredentialStore(new Credentials(token)));

            var repo = await client.Repository.Get(name, repositoryName);
            var request = new CommitRequest { Path = filename, Sha = version };
            return await client.Repository.Commit.GetAll(repo.Id, request);
        }
    }
}
