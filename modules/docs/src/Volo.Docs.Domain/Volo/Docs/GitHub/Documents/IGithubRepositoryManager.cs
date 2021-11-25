using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Octokit;
using Volo.Abp.DependencyInjection;
using Volo.Docs.GitHub.Documents.Version;
using Volo.Docs.Projects;

namespace Volo.Docs.GitHub.Documents
{
    public interface IGithubRepositoryManager : ITransientDependency
    {
        Task<string> GetFileRawStringContentAsync(string rawUrl, string token, string userAgent);

        Task<byte[]> GetFileRawByteArrayContentAsync(string rawUrl, string token, string userAgent);

        Task<IReadOnlyList<GithubVersion>> GetVersionsAsync(string name, string repositoryName, string token, GithubVersionProviderSource githubVersionProviderSource);

        Task<IReadOnlyList<GitHubCommit>> GetFileCommitsAsync(string name, string repositoryName, string version, string filename, string token);

        Task<GitHubCommit> GetSingleCommitsAsync(string name, string repositoryName, string sha, string token);
    }
}
