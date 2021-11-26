using System.Threading.Tasks;

namespace Volo.Abp.Studio.Nuget;

public interface INugetSourceCodeStore
{
    Task<string> GetCachedSourceCodeFilePathAsync(string name,
        string type,
        string version = null,
        bool includePreReleases = false);

    Task<string> GetCachedDllFilePathAsync(string name,
        string type,
        string version = null,
        bool includePreReleases = false,
        bool includeDependencies = false);
}
