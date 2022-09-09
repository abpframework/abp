using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Octokit;
using Octokit.Internal;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace Volo.Docs.GitHub.Documents.Version
{
    public class BranchGithubVersionProvider : IGithubVersionProvider, ITransientDependency
    {
        public async Task<List<GithubVersion>> GetVersions(string name, string repositoryName, string token)
        {
            try
            {
                var client = GetGitHubClient(name, token);
                var branches = await client.Repository.Branch.GetAll(name, repositoryName);

                return branches.Select(b => new GithubVersion { Name = b.Name }).ToList();
            }
            catch (Exception e)
            {
                var apiException = (Octokit.ApiException)e;
                if (apiException.ApiError?.Message == "Bad credentials" ||
                    apiException.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new AbpException("Invalid credentials for the GitHub repository: " + name + "/" + repositoryName + ". " +
                                           "The GitHub access token: " + token + " does not work!" + "\n\rError: " + e);
                }

                throw new AbpException("Could not get branches from GitHub repository: " + name + "/" + repositoryName + "\n\rError: " + e.Message);
            }
        }

        private static GitHubClient GetGitHubClient(string name, string token)
        {
            return token.IsNullOrWhiteSpace()
                ? new GitHubClient(new ProductHeaderValue(name))
                : new GitHubClient(new ProductHeaderValue(name), new InMemoryCredentialStore(new Credentials(token)));
        }
    }
}
