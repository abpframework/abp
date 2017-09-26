using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Volo.Abp.AspNetCore.App;

namespace Volo.Abp.AspNetCore.Mvc
{
    public abstract class AspNetCoreMvcTestBase : AbpAspNetCoreTestBase<Startup>
    {
        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            var contentRootPath = CalculateContentRootPath(
                "Volo.Abp.AspNetCore.Mvc.Tests.csproj",
                string.Format(
                    "Volo{0}Abp{0}AspNetCore{0}App",
                    Path.DirectorySeparatorChar
                )
            );

            return base.CreateWebHostBuilder()
                .UseContentRoot(contentRootPath);
        }

        private string CalculateContentRootPath(string projectFileName, string contentPath)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            while (!ContainsFile(currentDirectory, projectFileName))
            {
                currentDirectory = new DirectoryInfo(currentDirectory).Parent.FullName;
            }

            return Path.Combine(currentDirectory, contentPath);
        }

        private bool ContainsFile(string currentDirectory, string projectFileName)
        {
            return Directory
                .GetFiles(currentDirectory, "*.*", SearchOption.TopDirectoryOnly)
                .Any(f => Path.GetFileName(f) == projectFileName);
        }
    }
}