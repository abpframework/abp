using System.Collections.Generic;
using System.Threading.Tasks;

namespace Volo.Docs.GitHub.Documents.Version
{
    public interface IGithubVersionProvider
    {
        Task<List<GithubVersion>> GetVersions(string name, string repositoryName, string token);
    }
}
