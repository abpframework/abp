using System.IO;
using System.Linq;

namespace Volo.Abp.Cli.Bundling;

internal static class PathHelper
{
    internal static string GetWebAssemblyFrameworkFolderPath(string projectDirectory, string frameworkVersion)
    {
        return Path.Combine(projectDirectory, "bin", "Debug", frameworkVersion, "wwwroot", "_framework"); ;
    }

    internal static string GetWebAssemblyFilePath(string directory, string frameworkVersion, string projectFileName)
    {
        var outputDirectory = GetWebAssemblyFrameworkFolderPath(directory, frameworkVersion);
        return Path.Combine(outputDirectory, projectFileName + ".dll");
    }

    internal static string GetMauiBlazorAssemblyFilePath(string directory, string projectFileName)
    {
        return Directory.GetFiles(directory, "*.dll", SearchOption.AllDirectories).First(f => !f.Contains("android") && f.EndsWith(projectFileName + ".dll"));
    }

    internal static string GetWwwRootPath(string directory)
    {
        return Path.Combine(directory, "wwwroot");
    }
}
