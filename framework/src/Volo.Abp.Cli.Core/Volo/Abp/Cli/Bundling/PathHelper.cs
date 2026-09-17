using System;
using System.IO;
using System.Linq;

namespace Volo.Abp.Cli.Bundling;

static internal class PathHelper
{
    static internal string GetWebAssemblyFrameworkFolderPath(string projectDirectory, string frameworkVersion)
    {
        return Path.Combine(projectDirectory, "bin", "Debug", frameworkVersion, "wwwroot", "_framework");
    }

    static internal string GetWebAssemblyFilePath(string directory, string frameworkVersion, string projectFileName)
    {
        var outputDirectory = Path.Combine(directory, "bin", "Debug", frameworkVersion);
        var path = Path.Combine(outputDirectory, projectFileName + ".dll");
        return !File.Exists(path) ? null : path;
    }

    static internal string GetMauiBlazorAssemblyFilePath(string directory, string projectFileName)
    {
        return Directory.GetFiles(Path.Combine(directory, "bin"), "*.dll", SearchOption.AllDirectories).FirstOrDefault(f =>
            !f.Contains("android") &&
            !f.Contains("windows10") &&
            f.EndsWith(projectFileName + ".dll", StringComparison.OrdinalIgnoreCase));
    }

    static internal string GetWwwRootPath(string directory)
    {
        return Path.Combine(directory, "wwwroot");
    }
}
