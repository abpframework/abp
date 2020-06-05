using System;

namespace Volo.Abp.Cli.ProjectBuilding.Building
{
    public class GithubRepositoryInfo
    {
        public string RepositoryNameWithOrganization { get; }

        public string RepositoryName { get; }

        public string AccessToken { get; }

        public GithubRepositoryInfo(string repositoryNameWithOrganization, string accessToken)
        {
            if (!repositoryNameWithOrganization.Contains("/"))
            {
                throw new ApplicationException($"{nameof(repositoryNameWithOrganization)} '{repositoryNameWithOrganization}' is not valid! It should be formatted as 'organization-name/repository-name'." );
            }

            RepositoryNameWithOrganization = repositoryNameWithOrganization;
            RepositoryName = repositoryNameWithOrganization.Split('/')[1];
            AccessToken = accessToken;
        }
    }
}