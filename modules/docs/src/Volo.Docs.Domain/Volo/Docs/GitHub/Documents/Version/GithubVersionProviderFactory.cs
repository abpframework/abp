using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Docs.Projects;

namespace Volo.Docs.GitHub.Documents.Version
{
    public class GithubVersionProviderFactory : IGithubVersionProviderFactory, ITransientDependency
    {
        public IServiceProvider ServiceProvider { get; }

        public GithubVersionProviderFactory(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
        public IGithubVersionProvider Create(GithubVersionProviderSource source)
        {
            Type serviceType;

            switch (source)
            {
                case GithubVersionProviderSource.Branches:
                    serviceType = typeof(BranchGithubVersionProvider);
                    break;
                case GithubVersionProviderSource.Releases:
                default:
                    serviceType = typeof(ReleaseGithubVersionProvider);
                    break;
            }

            return (IGithubVersionProvider)ServiceProvider.GetRequiredService(serviceType);
        }
    }
}
