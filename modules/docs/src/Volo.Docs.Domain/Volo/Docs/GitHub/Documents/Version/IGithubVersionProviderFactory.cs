using Volo.Docs.Projects;

namespace Volo.Docs.GitHub.Documents.Version
{
    public interface IGithubVersionProviderFactory
    {
        IGithubVersionProvider Create(GithubVersionProviderSource source);
    }
}
