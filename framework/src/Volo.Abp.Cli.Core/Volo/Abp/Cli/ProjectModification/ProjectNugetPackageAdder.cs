using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Http;
using Volo.Abp.Cli.ProjectBuilding;
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
        protected DerivedClassFinder ModuleClassFinder { get; }
        protected ModuleClassDependcyAdder ModuleClassDependcyAdder { get; }
        protected IRemoteServiceExceptionHandler RemoteServiceExceptionHandler { get; }

        public ProjectNugetPackageAdder(
            IJsonSerializer jsonSerializer,
            ProjectNpmPackageAdder npmPackageAdder,
            DerivedClassFinder moduleClassFinder,
            ModuleClassDependcyAdder moduleClassDependcyAdder,
            IRemoteServiceExceptionHandler remoteServiceExceptionHandler)
        {
            JsonSerializer = jsonSerializer;
            NpmPackageAdder = npmPackageAdder;
            ModuleClassFinder = moduleClassFinder;
            ModuleClassDependcyAdder = moduleClassDependcyAdder;
            RemoteServiceExceptionHandler = remoteServiceExceptionHandler;
            Logger = NullLogger<ProjectNugetPackageAdder>.Instance;
        }

        public async Task AddAsync(string projectFile, string packageName, string version = null)
        {
            await AddAsync(
                projectFile,
                await FindNugetPackageInfoAsync(packageName),
                version
            );
        }

        public Task AddAsync(string projectFile, NugetPackageInfo package, string version = null)
        {
            var projectFileContent = File.ReadAllText(projectFile);

            if (projectFileContent.Contains($"\"{package.Name}\""))
            {
                return Task.CompletedTask;
            }

            if (version == null)
            {
                version = GetAbpVersionOrNull(projectFileContent);
            }

            using (DirectoryHelper.ChangeCurrentDirectory(Path.GetDirectoryName(projectFile)))
            {
                Logger.LogInformation($"Installing '{package.Name}' package to the project '{Path.GetFileNameWithoutExtension(projectFile)}'...");

                var versionOption = version == null ? "" : $" -v {version}";

                CmdHelper.Run("dotnet", $"add package {package.Name}{versionOption}");

                var moduleFiles = ModuleClassFinder.Find(projectFile, "AbpModule");
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

            return Task.CompletedTask;
        }

        private string GetAbpVersionOrNull(string projectFileContent)
        {
            var doc = new XmlDocument() { PreserveWhitespace = true };

            doc.Load(StreamHelper.GenerateStreamFromString(projectFileContent));

            var nodes = doc.SelectNodes("/Project/ItemGroup/PackageReference[starts-with(@Include, 'Volo.')]");

            return nodes?[0]?.Attributes?["Version"]?.Value;
        }

        protected virtual async Task<NugetPackageInfo> FindNugetPackageInfoAsync(string packageName)
        {
            using (var client = new CliHttpClient())
            {
                var url = $"{CliUrls.WwwAbpIo}api/app/nugetPackage/byName/?name=" + packageName;

                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        throw new CliUsageException($"'{packageName}' nuget package could not be found!");
                    }

                    await RemoteServiceExceptionHandler.EnsureSuccessfulHttpResponseAsync(response);
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<NugetPackageInfo>(responseContent);
            }
        }
    }
}
