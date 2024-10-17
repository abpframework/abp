using System.IO;

namespace Volo.Abp.AspNetCore.TestBase;

public static class GetWebProjectContentRootPathHelper
{
    public static string Get(string webProjectName)
    {
        var currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());

        while (currentDirectory != null && Directory.GetParent(currentDirectory.FullName) != null)
        {
            currentDirectory = Directory.GetParent(currentDirectory.FullName);
            if (currentDirectory == null)
            {
                continue;
            }

            var files = currentDirectory.GetFiles(webProjectName, SearchOption.AllDirectories);
            if (files.Length > 0)
            {
                return files[0].DirectoryName!;
            }
        }

        throw new AbpException($"Web project({webProjectName}) not found!");
    }
}
