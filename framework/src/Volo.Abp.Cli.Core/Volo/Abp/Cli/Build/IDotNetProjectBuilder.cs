using System.Collections.Generic;

namespace Volo.Abp.Cli.Build
{
    public interface IDotNetProjectBuilder
    {
        List<string> Build(List<DotNetProjectInfo> projects, int maxParallelBuildCount, string arguments);
    }
}
