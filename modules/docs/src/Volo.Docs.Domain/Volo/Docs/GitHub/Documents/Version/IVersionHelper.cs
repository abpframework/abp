using System.Collections.Generic;
using Volo.Docs.Projects;

namespace Volo.Docs.GitHub.Documents.Version
{
    public interface IVersionHelper
    {
        List<string> OrderByDescending(List<string> versions);

        List<VersionInfo> OrderByDescending(List<VersionInfo> versions);

        bool IsPreRelease(string version);
    }
}
