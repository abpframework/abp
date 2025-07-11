using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json.Linq;
using Volo.Abp.Cli.Commands.Services;
using Volo.Abp.Cli.Http;
using Volo.Abp.Cli.LIbs;
using Volo.Abp.Cli.ProjectBuilding;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;
using Volo.Abp.IO;
using Volo.Abp.Json;

namespace Volo.Abp.Cli.ProjectModification;

public class ProjectNpmPackageAdder : ITransientDependency
{
    public IJsonSerializer JsonSerializer { get; }
    public SourceCodeDownloadService SourceCodeDownloadService { get; }
    public AngularSourceCodeAdder AngularSourceCodeAdder { get; }
    public IRemoteServiceExceptionHandler RemoteServiceExceptionHandler { get; }
    public IInstallLibsService InstallLibsService { get; }
    public ICmdHelper CmdHelper { get; }
    public CliHttpClientFactory CliHttpClientFactory { get; }
    public INpmPackageInfoProvider NpmPackageInfoProvider { get; }

    public ILogger<ProjectNpmPackageAdder> Logger { get; set; }

    public ProjectNpmPackageAdder(
        IJsonSerializer jsonSerializer,
        SourceCodeDownloadService sourceCodeDownloadService,
        AngularSourceCodeAdder angularSourceCodeAdder,
        IRemoteServiceExceptionHandler remoteServiceExceptionHandler,
        IInstallLibsService installLibsService,
        ICmdHelper cmdHelper,
        CliHttpClientFactory cliHttpClientFactory,
        INpmPackageInfoProvider npmPackageInfoProvider)
    {
        JsonSerializer = jsonSerializer;
        SourceCodeDownloadService = sourceCodeDownloadService;
        AngularSourceCodeAdder = angularSourceCodeAdder;
        RemoteServiceExceptionHandler = remoteServiceExceptionHandler;
        InstallLibsService = installLibsService;
        CmdHelper = cmdHelper;
        CliHttpClientFactory = cliHttpClientFactory;
        NpmPackageInfoProvider = npmPackageInfoProvider;

        Logger = NullLogger<ProjectNpmPackageAdder>.Instance;
    }

    public async Task AddNpmPackageAsync(string directory, string npmPackageName, string version = null,
        bool withSourceCode = false)
    {
        await AddNpmPackageAsync(
            directory,
            await FindNpmPackageInfoAsync(npmPackageName),
            version,
            withSourceCode
        );
    }

    public async Task AddNpmPackageAsync(string directory, NpmPackageInfo npmPackage, string version = null,
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
                CmdHelper.RunCmd("npx yarn add " + npmPackage.Name + versionPostfix);
            }
        }
        else
        {
            Logger.LogInformation($"'{npmPackage.Name}' is already installed.");
        }

        if (withSourceCode && await DownloadAngularSourceCode(directory, npmPackage, version))
        {
            await AngularSourceCodeAdder.AddAsync(directory, npmPackage);
        }
    }

    protected virtual async Task<bool> DownloadAngularSourceCode(string angularDirectory, NpmPackageInfo package,
        string version = null)
    {
        var downloadablePackages = await NpmPackageInfoProvider.GetPackageListAsync();
        if (!downloadablePackages.Exists(p => p.Name == package.Name))
        {
            Logger.LogError($"'{package.Name}' is not downloadable!");
            Logger.LogInformation("The downloadable packages are: ");
            foreach (var downloadablePackage in downloadablePackages)
            {
                Logger.LogInformation($"> {downloadablePackage.Name}");
            }
            return false;
        }

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

        return true;
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
            CmdHelper.RunCmd("npx yarn add " + npmPackage.Name + versionPostfix);

            if (skipInstallingLibs)
            {
                return;
            }

            Logger.LogInformation("Installing client-side packages...");
            await InstallLibsService.InstallLibsAsync(directory);
        }
    }

    public async Task RemoveMvcPackageAsync(string directory, NpmPackageInfo npmPackage,
        bool skipInstallingLibs = false)
    {
        var packageJsonFilePath = Path.Combine(directory, "package.json");
        if (!File.Exists(packageJsonFilePath) ||
            !File.ReadAllText(packageJsonFilePath).Contains($"\"{npmPackage.Name}\""))
        {
            return;
        }

        Logger.LogInformation($"Removing '{npmPackage.Name}' package from the project '{packageJsonFilePath}'...");


        using (DirectoryHelper.ChangeCurrentDirectory(directory))
        {
            Logger.LogInformation("yarn remove " + npmPackage.Name);
            CmdHelper.RunCmd("npx yarn remove " + npmPackage.Name);

            if (skipInstallingLibs)
            {
                return;
            }

            Logger.LogInformation("Installing client-side packages...");
            await InstallLibsService.InstallLibsAsync(directory);
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
                if ((package.Name.StartsWith("@abp/") || package.Name.StartsWith("@volo/")) && !package.Name.Contains("leptonx"))
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
        var client = CliHttpClientFactory.CreateClient();

        using (var response = await client.GetAsync(url, CliHttpClientFactory.GetCancellationToken()))
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
