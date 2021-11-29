using System.IO;

namespace Volo.Abp.Cli.Bundling
{
    internal static class PathHelper
    {
        internal static string GetFrameworkFolderPath(string projectDirectory, string frameworkVersion)
        {
            return Path.Combine(projectDirectory, "bin", "Debug", frameworkVersion, "wwwroot", "_framework"); ;
        }

        internal static string GetAssemblyFilePath(string directory, string frameworkVersion, string projectFileName)
        {
            var outputDirectory = GetFrameworkFolderPath(directory, frameworkVersion);
            return Path.Combine(outputDirectory, projectFileName + ".dll");
        }

        internal static string GetWwwRootPath(string directory)
        {
            return Path.Combine(directory, "wwwroot");
        }
    }
}
