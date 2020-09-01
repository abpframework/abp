using System.Collections.Generic;

namespace Volo.Abp.Cli.Build
{
    public interface IBuildStatusGenerator
    {
        GitRepositoryBuildStatus Generate(
            GitRepository gitRepository,
            List<DotNetProjectInfo> changedProjects,
            List<string> buildSucceededProjects
        );
    }
}
