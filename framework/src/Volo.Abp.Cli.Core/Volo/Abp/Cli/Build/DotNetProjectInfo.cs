using System;
using System.Collections.Generic;

namespace Volo.Abp.Cli.Build;

[Serializable]
public class DotNetProjectInfo
{
    public string RepositoryName { get; set; }

    public string CsProjPath { get; set; }

    public bool ShouldBuild { get; set; }

    public List<DotNetProjectInfo> Dependencies { get; set; }

    public DotNetProjectInfo(string repositoryName, string csProjPath, bool shouldBuild)
    {
        RepositoryName = repositoryName;
        CsProjPath = csProjPath;
        ShouldBuild = shouldBuild;
        Dependencies = new List<DotNetProjectInfo>();
    }
}
