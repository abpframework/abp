using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.Cli.Build;

public static class DotNetProjectInfoExtensions
{
    public static void MarkForBuild(this List<DotNetProjectInfo> projects, string repositoryName, string csProjPath)
    {
        var project = projects.FirstOrDefault(e =>
            e.RepositoryName == repositoryName && e.CsProjPath == csProjPath
        );

        if (project == null)
        {
            return;
        }

        project.ShouldBuild = true;
    }

    public static void MarkForBuild(this List<DotNetProjectInfo> projects, DotNetProjectInfo project)
    {
        projects.MarkForBuild(project.RepositoryName, project.CsProjPath);
    }

    public static bool IsMarkedForBuild(this List<DotNetProjectInfo> projects, string repositoryName,
        string csProjPath)
    {
        var project = projects.FirstOrDefault(e =>
            e.RepositoryName == repositoryName && e.CsProjPath == csProjPath
        );

        if (project == null)
        {
            return false;
        }

        return project.ShouldBuild;
    }

    public static bool IsMarkedForBuild(this List<DotNetProjectInfo> projects, DotNetProjectInfo project)
    {
        return projects.IsMarkedForBuild(project.RepositoryName, project.CsProjPath);
    }
}
