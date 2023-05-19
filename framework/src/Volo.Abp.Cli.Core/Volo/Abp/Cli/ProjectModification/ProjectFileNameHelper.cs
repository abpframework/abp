using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Volo.Abp.Cli.ProjectModification;

public static class ProjectFileNameHelper
{

    public static string GetAssemblyNameFromProjectPath(string projectFile)
    {
        return projectFile
            .Substring(projectFile.LastIndexOf(Path.DirectorySeparatorChar) + 1)
            .RemovePostFix(StringComparison.OrdinalIgnoreCase, ".csproj");
    }
}
