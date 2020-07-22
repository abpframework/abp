using System.Collections.Generic;
using Volo.Docs.Projects;

namespace Volo.Docs.Version
{
    public interface IVersionHelper
    {
        List<string> OrderByDescending(List<string> versions);

        List<VersionInfoDto> OrderByDescending(List<VersionInfoDto> versions);

        bool IsPreRelease(string version);
    }
}
