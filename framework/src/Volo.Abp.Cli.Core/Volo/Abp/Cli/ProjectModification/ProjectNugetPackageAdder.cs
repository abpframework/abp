using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;
using Volo.Abp.IO;
using Volo.Abp.Json;

namespace Volo.Abp.Cli.ProjectModification
{
    public class ProjectNugetPackageAdder : ITransientDependency
    {
        public ILogger<ProjectNugetPackageAdder> Logger { get; set; }

        protected IJsonSerializer JsonSerializer { get; }
        protected ProjectNpmPackageAdder NpmPackageAdder { get; }
        protected AbpModuleClassFinder ModuleClassFinder { get; }
        protected ModuleClassDependcyAdder ModuleClassDependcyAdder { get; }

        public ProjectNugetPackageAdder(
            IJsonSerializer jsonSerializer,
            ProjectNpmPackageAdder npmPackageAdder,
            AbpModuleClassFinder moduleClassFinder,
            ModuleClassDependcyAdder moduleClassDependcyAdder)
        {
            JsonSerializer = jsonSerializer;
            NpmPackageAdder = npmPackageAdder;
            ModuleClassFinder = moduleClassFinder;
            ModuleClassDependcyAdder = moduleClassDependcyAdder;
            Logger = NullLogger<ProjectNugetPackageAdder>.Instance;
        }

        public async Task AddAsync(string projectFile, string packageName)
        {
            await AddAsync(
                projectFile,
                await FindNugetPackageInfoAsync(packageName)
            );
        }

        public async Task AddAsync(string projectFile, NugetPackageInfo package)
        {
            using (DirectoryHelper.ChangeCurrentDirectory(Path.GetDirectoryName(projectFile)))
            {
                Logger.LogInformation($"Installing '{package.Name}' package to the project '{Path.GetFileNameWithoutExtension(projectFile)}'...");

                CmdHelper.Run("dotnet", "add package " + package.Name);

                var moduleFiles = ModuleClassFinder.Find(projectFile);
                if (moduleFiles.Count == 0)
                {
                    throw new CliUsageException($"Could not find a class derived from AbpModule in the project {projectFile}");
                }

                if (moduleFiles.Count > 1)
                {
                    throw new CliUsageException($"There are multiple classes derived from AbpModule in the project {projectFile}: " + moduleFiles.JoinAsString(", "));
                }

                ModuleClassDependcyAdder.Add(moduleFiles.First(), package.ModuleClass);

                Logger.LogInformation("Successfully installed.");
            }
        }

        protected virtual async Task<NugetPackageInfo> FindNugetPackageInfoAsync(string moduleName)
        {
            using (var client = new HttpClient())
            {
                var url = "https://localhost:44328/api/app/nugetPackage/byName/?name=" + moduleName;

                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        throw new CliUsageException($"'{moduleName}' nuget package could not be found!");
                    }

                    throw new Exception($"ERROR: Remote server returns '{response.StatusCode}'");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<NugetPackageInfo>(responseContent);
            }
        }
    }
}