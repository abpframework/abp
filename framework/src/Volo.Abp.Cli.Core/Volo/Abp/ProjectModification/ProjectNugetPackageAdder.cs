using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace Volo.Abp.ProjectModification
{
    public class ProjectNugetPackageAdder : ITransientDependency
    {
        public ILogger<ProjectNugetPackageAdder> Logger { get; set; }

        protected IJsonSerializer JsonSerializer { get; }
        protected ProjectNpmPackageAdder NpmPackageAdder { get; }

        public ProjectNugetPackageAdder(
            IJsonSerializer jsonSerializer,
            ProjectNpmPackageAdder npmPackageAdder)
        {
            JsonSerializer = jsonSerializer;
            NpmPackageAdder = npmPackageAdder;
            Logger = NullLogger<ProjectNugetPackageAdder>.Instance;
        }

        public async Task AddAsync(string projectFile, string packageName)
        {
            var packageInfo = await FindNugetPackageInfoAsync(packageName);
            if (packageInfo == null)
            {
                return;
            }

            Logger.LogInformation($"Installing '{packageName}' package to the project '{Path.GetFileNameWithoutExtension(projectFile)}'...");
            var procStartInfo = new ProcessStartInfo("dotnet", "add package " + packageName);
            Process.Start(procStartInfo).WaitForExit();

            var moduleFile = new AbpModuleFinder().Find(projectFile).First();
            new DependsOnAdder().Add(moduleFile, packageInfo.ModuleClass);

            if (packageInfo.DependedNpmPackage != null)
            {
                await NpmPackageAdder.AddAsync(Path.GetDirectoryName(projectFile), packageInfo.DependedNpmPackage);
            }

            Logger.LogInformation("Successfully installed.");
        }

        private async Task<NugetPackageInfo> FindNugetPackageInfoAsync(string moduleName)
        {
            using (var client = new HttpClient())
            {
                var url = "https://localhost:44328/api/app/nugetPackage/byName/?name=" + moduleName;

                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        Logger.LogError($"ERROR: '{moduleName}' module could not be found.");
                    }
                    else
                    {
                        Logger.LogError($"ERROR: Remote server returns '{response.StatusCode}'");
                    }

                    return null;
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<NugetPackageInfo>(responseContent);
            }
        }
    }
}