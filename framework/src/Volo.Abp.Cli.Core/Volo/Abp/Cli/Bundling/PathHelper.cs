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
        return Path.Combine(outputDirectory, projectFileName + ".dll");
    }

    static internal string GetMauiBlazorAssemblyFilePath(string directory, string projectFileName)
    {
        return Directory.GetFiles(directory, "*.dll", SearchOption.AllDirectories).First(f => !f.Contains("android") && f.EndsWith(projectFileName + ".dll"));
    }

    static internal string GetWwwRootPath(string directory)
    {
        return Path.Combine(directory, "wwwroot");
    }
}
