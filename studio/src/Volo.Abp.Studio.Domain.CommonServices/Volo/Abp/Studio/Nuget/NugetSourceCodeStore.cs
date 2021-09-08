using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Cli.NuGet;
using Volo.Abp.Cli.ProjectBuilding;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Studio.Helpers;

namespace Volo.Abp.Studio.Nuget
{
    [Dependency(ReplaceServices = true)]
    public class NugetSourceCodeStore : ISourceCodeStore, INugetSourceCodeStore, ITransientDependency
    {
        private readonly NuGetService _nuGetService;
        private readonly NugetPackageCacheManager _nugetPackageCacheManager;
        private readonly ICmdHelper _cmdHelper;

        public NugetSourceCodeStore(
            NuGetService nuGetService,
            NugetPackageCacheManager nugetPackageCacheManager,
            ICmdHelper cmdHelper)
        {
            _nuGetService = nuGetService;
            _nugetPackageCacheManager = nugetPackageCacheManager;
            _cmdHelper = cmdHelper;
        }

        public async Task<TemplateFile> GetAsync(
            string name,
            string type,
            string version = null,
            string templateSource = null,
            bool includePreReleases = false)
        {
            name = GetNugetPackageName(name, type);

           var latestVersion =  await GetLatestVersionAsync(name, includePreReleases);

            if (version == null)
            {
                version = latestVersion;
            }

            var localCachedFilePath = await GetLocalCacheSourceCodeFilePathInternal(name, version);

            return new TemplateFile(await File.ReadAllBytesAsync(localCachedFilePath), version, latestVersion, version);
        }

        public async Task<string> GetCachedSourceCodeFilePathAsync(string name, string type, string version = null,
            bool includePreReleases = false)
        {
            name = GetNugetPackageName(name, type);

            if (version == null)
            {
                version = await GetLatestVersionAsync(name, includePreReleases);
            }

            return await GetLocalCacheSourceCodeFilePathInternal(name, version);
        }

        public async Task<string> GetCachedDllFilePathAsync(string name, string type, string version = null, bool includePreReleases = false, bool includeDependencies = false)
        {
            if (type == SourceCodeTypes.Template)
            {
                name = TemplateNugetPackageInfoProvider.GetNugetPackageName(name);
            }

            if (version == null)
            {
                version = await GetLatestVersionAsync(name, includePreReleases);
            }

            var localDllFolder = Path.Combine(
                GetLocalNugetCachePath(),
                name,
                version,
                "lib");

            if (!Directory.Exists(localDllFolder) ||
                (includeDependencies && !Directory.GetFiles(localDllFolder, $"*Volo.Abp.Studio.ModuleInstaller.dll", SearchOption.AllDirectories).Any()))
            {
                if (includeDependencies)
                {
                    var temporaryFolder = await _nugetPackageCacheManager.CachePackageAsync(name, version, false);

                    var outputFolder = Path.GetDirectoryName(
                        Directory
                            .GetFiles(localDllFolder, $"*{name}.dll", SearchOption.AllDirectories)
                            .First()
                        );

                    _cmdHelper.RunCmdAndGetOutput($"dotnet build -o {outputFolder}", temporaryFolder);

                    Directory.Delete(temporaryFolder, true);
                }
                else
                {
                    await _nugetPackageCacheManager.CachePackageAsync(name, version);
                }
            }

            if (!Directory.Exists(localDllFolder))
            {
                return null;
            }

            return Directory.GetFiles(localDllFolder, $"{name}.dll", SearchOption.AllDirectories).First();
        }

        private async Task<string> GetLatestVersionAsync(string nugetPackage, bool includePreReleases)
        {
            var v = await _nuGetService.GetLatestVersionOrNullAsync(nugetPackage, includePreReleases);

            return v.ToString();
        }

        private async Task<string> GetLocalCacheSourceCodeFilePathInternal(string name, string version)
        {
            var localCacheFile = Path.Combine(
                GetLocalNugetCachePath(),
                name,
                version,
                "content",
                $"{name}.zip");


            if (!File.Exists(localCacheFile))
            {
                await _nugetPackageCacheManager.CachePackageAsync(name, version);
            }

            return localCacheFile;
        }

        private string GetLocalNugetCachePath()
        {
            return Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                        ".nuget",
                        "packages");
        }

        private string GetNugetPackageName(string name, string type)
        {
            if (type == SourceCodeTypes.Template)
            {
                return TemplateNugetPackageInfoProvider.GetNugetPackageName(name);
            }

            if (type == SourceCodeTypes.Module)
            {
                return $"{name}.SourceCode";
            }

            return name;
        }
    }
}
