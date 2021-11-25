using System.Collections.Generic;

namespace Volo.Abp.Cli.Build;

public interface IChangedProjectFinder
{
    List<DotNetProjectInfo> FindByRepository(DotNetProjectBuildConfig buildConfig);
}
