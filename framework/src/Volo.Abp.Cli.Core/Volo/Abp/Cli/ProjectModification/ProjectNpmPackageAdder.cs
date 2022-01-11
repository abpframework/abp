using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json.Linq;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.Commands;
using Volo.Abp.Cli.Commands.Services;
using Volo.Abp.Cli.Http;
using Volo.Abp.Cli.ProjectBuilding;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;
using Volo.Abp.IO;
using Volo.Abp.Json;

namespace Volo.Abp.Cli.ProjectModification
{
    public class ProjectNpmPackageAdder : ITransientDependency
    {
        public IJsonSerializer JsonSerializer { get; }
        public SourceCodeDownloadService SourceCodeDownloadService { get; }
        public AngularSourceCodeAdder AngularSourceCodeAdder { get; }
        public IRemoteServiceExceptionHandler RemoteServiceExceptionHandler { get; }
        public InstallLibsCommand InstallLibsCommand { get; }
        public ICmdHelper CmdHelper { get; }
        private readonly CliHttpClientFactory _cliHttpClientFactory;
        public ILogger<ProjectNpmPackageAdder> Logger { get; set; }

        public ProjectNpmPackageAdder(CliHttpClientFactory cliHttpClientFactory,
            IJsonSerializer jsonSerializer,
            SourceCodeDownloadService sourceCodeDownloadService,
            AngularSourceCodeAdder angularSourceCodeAdder,
            IRemoteServiceExceptionHandler remoteServiceExceptionHandler,
            InstallLibsCommand ınstallLibsCommand,
            ICmdHelper cmdHelper)
        {
            JsonSerializer = jsonSerializer;
            SourceCodeDownloadService = sourceCodeDownloadService;
            AngularSourceCodeAdder = angularSourceCodeAdder;
            RemoteServiceExceptionHandler = remoteServiceExceptionHandler;
            InstallLibsCommand = ınstallLibsCommand;
            CmdHelper = cmdHelper;
            _cliHttpClientFactory = cliHttpClientFactory;
            Logger = NullLogger<ProjectNpmPackageAdder>.Instance;
        }

        public async Task AddAngularPackageAsync(string directory, string npmPackageName, string version = null,
            bool withSourceCode = false)
        {
            await AddAngularPackageAsync(
                directory,
                await FindNpmPackageInfoAsync(npmPackageName),
                version,
                withSourceCode
            );
        }

        public async Task AddAngularPackageAsync(string directory, NpmPackageInfo npmPackage, string version = null,
            bool withSourceCode = false)
        {
            var packageJsonFilePath = Path.Combine(directory, "package.json");
            if (!File.Exists(packageJsonFilePath))
            {
                Logger.LogError($"package.json not found!");
                return;
            }

            Logger.LogInformation($"Installing '{npmPackage.Name}' package to the project '{packageJsonFilePath}'...");

            if (!File.ReadAllText(packageJsonFilePath).Contains($"\"{npmPackage.Name}\""))
            {
                var versionPostfix = version != null ? $"@{version}" : string.Empty;

                using (DirectoryHelper.ChangeCurrentDirectory(directory))
                {
                    Logger.LogInformation("yarn add " + npmPackage.Name + versionPostfix);
                    CmdHelper.RunCmd("yarn add " + npmPackage.Name + versionPostfix);
                }
            }
            else
            {
                Logger.LogInformation($"'{npmPackage.Name}' is already installed.");
            }

            if (withSourceCode)
            {
                await DownloadAngularSourceCode(directory, npmPackage, version);
                await AngularSourceCodeAdder.AddAsync(directory, npmPackage);
            }
        }

        protected virtual async Task DownloadAngularSourceCode(string angularDirectory, NpmPackageInfo package,
            string version = null)
        {
            var targetFolder = Path.Combine(angularDirectory, "projects",
                package.Name.RemovePreFix("@").Replace("/", "-"));

            if (Directory.Exists(targetFolder))
            {
                Directory.Delete(targetFolder, true);
            }

            await SourceCodeDownloadService.DownloadNpmPackageAsync(
                package.Name,
                targetFolder,
                version
            );
        }

        public async Task AddMvcPackageAsync(string directory, NpmPackageInfo npmPackage, string version = null,
            bool skipInstallingLibs = false)
        {
            var packageJsonFilePath = Path.Combine(directory, "package.json");
            if (!File.Exists(packageJsonFilePath) ||
                File.ReadAllText(packageJsonFilePath).Contains($"\"{npmPackage.Name}\""))
            {
                return;
            }

            Logger.LogInformation($"Installing '{npmPackage.Name}' package to the project '{packageJsonFilePath}'...");


            if (version == null)
            {
                version = DetectAbpVersionOrNull(Path.Combine(directory, "package.json"));
            }

            var versionPostfix = version != null ? $"@{version}" : string.Empty;

            using (DirectoryHelper.ChangeCurrentDirectory(directory))
            {
                Logger.LogInformation("yarn add " + npmPackage.Name + versionPostfix);
                CmdHelper.RunCmd("yarn add " + npmPackage.Name + versionPostfix);

                if (skipInstallingLibs)
                {
                    return;
                }

                await InstallLibsCommand.ExecuteAsync(
                    new CommandLineArgs("install-libs")
                );
            }
        }

        private string DetectAbpVersionOrNull(string packageJsonFile)
        {
            if (string.IsNullOrEmpty(packageJsonFile) ||
                !File.Exists(packageJsonFile))
            {
                return null;
            }

            try
            {
                var packageJsonFileContent = File.ReadAllText(packageJsonFile);
                var packageJsonObject = JObject.Parse(packageJsonFileContent);
                var dependenciesObject = (JObject)packageJsonObject["dependencies"];

                if (dependenciesObject == null)
                {
                    return null;
                }

                var packages = dependenciesObject.Children<JProperty>();

                foreach (var package in packages)
                {
                    if (package.Name.StartsWith("@abp/") || package.Name.StartsWith("@volo/"))
                    {
                        return package.Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogWarning("Cannot detect ABP package version. " + ex.Message);
            }

            return null;
        }

        private async Task<NpmPackageInfo> FindNpmPackageInfoAsync(string packageName)
        {
            var url = $"{CliUrls.WwwAbpIo}api/app/npmPackage/byName/?name=" + packageName;
            var client = _cliHttpClientFactory.CreateClient();

            using (var response = await client.GetAsync(url, _cliHttpClientFactory.GetCancellationToken()))
            {
                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        throw new CliUsageException($"'{packageName}' npm package could not be found!");
                    }

                    await RemoteServiceExceptionHandler.EnsureSuccessfulHttpResponseAsync(response);
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<NpmPackageInfo>(responseContent);
            }
        }
    }
}
