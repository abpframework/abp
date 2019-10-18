using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Demo
{
    public abstract class AbpAspNetCoreMvcUiBootstrapDemoTestBase : AbpAspNetCoreTestBase<TestStartup>
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            return base.CreateHostBuilder()
                .UseContentRoot(CalculateContentRootPath());
        }

        private static string CalculateContentRootPath()
        {
            return CalculateContentRootPath(
                "Volo.Abp.sln",
                string.Format(
                    "test{0}Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Demo",
                    Path.DirectorySeparatorChar
                )
            );
        }

        private static string CalculateContentRootPath(
            string testFileNameInTheRootFolder,
            string relativeContentPath)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            while (!ContainsFile(currentDirectory, testFileNameInTheRootFolder))
            {
                currentDirectory = new DirectoryInfo(currentDirectory).Parent.FullName;
            }

            if (!relativeContentPath.IsNullOrWhiteSpace())
            {
                currentDirectory = Path.Combine(currentDirectory, relativeContentPath);
            }

            return currentDirectory;
        }

        private static bool ContainsFile(string currentDirectory, string projectFileName)
        {
            return Directory
                .GetFiles(currentDirectory, "*.*", SearchOption.TopDirectoryOnly)
                .Any(f => Path.GetFileName(f) == projectFileName);
        }
    }
}