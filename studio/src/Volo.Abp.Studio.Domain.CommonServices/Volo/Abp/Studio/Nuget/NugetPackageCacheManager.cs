using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Studio.Helpers;

namespace Volo.Abp.Studio.Nuget
{
    public class NugetPackageCacheManager : ITransientDependency
    {
        private readonly ICmdHelper _cmdHelper;

        public NugetPackageCacheManager(ICmdHelper cmdHelper)
        {
            _cmdHelper = cmdHelper;
        }

        public async Task<string> CachePackageAsync(string packageName, string version, bool deleteAfter = true)
        {
            var temporaryFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            Directory.CreateDirectory(temporaryFolder);

            _cmdHelper.RunCmdAndGetOutput("dotnet new console -lang c#", temporaryFolder);
            _cmdHelper.RunCmdAndGetOutput($"dotnet add package {packageName} --version {version}", temporaryFolder);

            if (deleteAfter)
            {
                Directory.Delete(temporaryFolder, true);
            }

            return temporaryFolder;
        }
    }
}
