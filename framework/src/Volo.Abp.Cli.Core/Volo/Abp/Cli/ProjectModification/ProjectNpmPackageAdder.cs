using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public NpmHelper NpmHelper { get; }
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
        INpmPackageInfoProvider npmPackageInfoProvider,
        NpmHelper npmHelper)
    {
        JsonSerializer = jsonSerializer;
        SourceCodeDownloadService = sourceCodeDownloadService;
        AngularSourceCodeAdder = angularSourceCodeAdder;
        RemoteServiceExceptionHandler = remoteServiceExceptionHandler;
        InstallLibsService = installLibsService;
        CmdHelper = cmdHelper;
        CliHttpClientFactory = cliHttpClientFactory;
        NpmPackageInfoProvider = npmPackageInfoProvider;
        NpmHelper = npmHelper;

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
        if (npmPackage.ApplicationType.HasFlag(NpmApplicationType.React))
        {
            await AddReactPackageAsync(directory, npmPackage, version, withSourceCode);
            return;
        }

        var packageJsonFilePath = Path.Combine(directory, "package.json");
        if (!File.Exists(packageJsonFilePath))
        {
            Logger.LogError($"package.json not found!");
            return;
        }

        NpmHelper.EnsureSafePackageName(npmPackage.Name);
        NpmHelper.EnsureSafeVersion(version);

        Logger.LogInformation($"Installing '{npmPackage.Name}' package to the project '{packageJsonFilePath}'...");

        if (!File.ReadAllText(packageJsonFilePath).Contains($"\"{npmPackage.Name}\""))
        {
            var versionPostfix = version != null ? $"@{version}" : string.Empty;

            using (DirectoryHelper.ChangeCurrentDirectory(directory))
            {
                Logger.LogInformation("yarn add " + npmPackage.Name + versionPostfix);
                CmdHelper.RunCmd(NpmHelper.GetYarnCommand(directory, "add " + npmPackage.Name + versionPostfix));
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

    /// <summary>
    /// React packages are plain NPM dependencies: nothing is registered in the application code.
    /// The package is added with the package manager of the project and, unless a version is given,
    /// with the version of the ABP packages the project already uses.
    /// The source code can be downloaded like for the other NPM packages, but it is not wired into the project.
    /// </summary>
    protected virtual async Task AddReactPackageAsync(string directory, NpmPackageInfo npmPackage, string version,
        bool withSourceCode)
    {
        NpmHelper.EnsureSafePackageName(npmPackage.Name);
        NpmHelper.EnsureSafeVersion(version);

        directory = FindReactProjectDirectory(directory, npmPackage);

        InstallReactPackage(directory, npmPackage, version);

        // The source code is downloaded the same way as for the other NPM packages (the license is checked
        // by abp.io). Only the registration that follows it for Angular is skipped: it writes angular.json
        // and tsconfig path mappings, which a React project doesn't have.
        if (withSourceCode && await DownloadAngularSourceCode(directory, npmPackage, version))
        {
            Logger.LogInformation(
                $"The source code of '{npmPackage.Name}' has been downloaded into the 'projects' folder of '{directory}'. " +
                "The project still uses the NPM package; reference the downloaded source code yourself if you want to use it."
            );
        }
    }

    private void InstallReactPackage(string directory, NpmPackageInfo npmPackage, string version)
    {
        var packageJsonFilePath = Path.Combine(directory, "package.json");

        Logger.LogInformation($"Installing '{npmPackage.Name}' package to the project '{packageJsonFilePath}'...");

        if (File.ReadAllText(packageJsonFilePath).Contains($"\"{npmPackage.Name}\""))
        {
            Logger.LogInformation($"'{npmPackage.Name}' is already installed.");
            return;
        }

        version ??= DetectSafeAbpVersionOrNull(packageJsonFilePath);

        var command = NpmHelper.GetAddPackageCommand(directory, npmPackage.Name, version);

        Logger.LogInformation(command);
        CmdHelper.RunCmd(command, out var exitCode, directory);

        if (exitCode != 0)
        {
            throw new AbpException(
                $"Failed to add '{npmPackage.Name}' to '{packageJsonFilePath}'. '{command}' exited with code {exitCode}."
            );
        }
    }

    /// <summary>
    /// Uses the given directory when it has a package.json. Otherwise looks for the React project below it
    /// (for example when the command runs in the solution folder) and never guesses between several of them.
    /// </summary>
    protected virtual string FindReactProjectDirectory(string directory, NpmPackageInfo npmPackage)
    {
        if (!Directory.Exists(directory))
        {
            throw new CliUsageException(
                $"The directory '{directory}' doesn't exist. Check the value of the '--working-directory' option."
            );
        }

        if (File.Exists(Path.Combine(directory, "package.json")))
        {
            return directory;
        }

        var reactProjectDirectories = ReactProjectFinder.Find(directory);

        if (reactProjectDirectories.Count == 1)
        {
            Logger.LogInformation($"Found the React project: {reactProjectDirectories[0]}");
            return reactProjectDirectories[0];
        }

        if (reactProjectDirectories.Count == 0)
        {
            throw new CliUsageException(
                $"No React project was found in '{directory}' to add '{npmPackage.Name}'. " +
                "Run the command in the folder of the React project or specify it with the '--working-directory' option."
            );
        }

        throw new CliUsageException(
            $"More than one React project was found in '{directory}'. " +
            $"Specify the one to add '{npmPackage.Name}' to with the '--working-directory' option:" +
            Environment.NewLine +
            reactProjectDirectories
                .Select(d => "  " + Path.GetRelativePath(directory, d))
                .JoinAsString(Environment.NewLine)
        );
    }

    private string DetectSafeAbpVersionOrNull(string packageJsonFilePath)
    {
        var version = DetectAbpVersionOrNull(packageJsonFilePath);
        if (version == null || NpmHelper.IsSafeVersion(version))
        {
            return version;
        }

        // For example a local "file:" reference, which cannot be used as a version of another package.
        Logger.LogWarning(
            $"Cannot use the ABP package version '{NpmHelper.SanitizeForLog(version)}' of the project. " +
            "The latest version will be installed."
        );

        return null;
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
        NpmHelper.EnsureSafePackageName(npmPackage.Name);

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

        NpmHelper.EnsureSafeVersion(version);

        var versionPostfix = version != null ? $"@{version}" : string.Empty;

        using (DirectoryHelper.ChangeCurrentDirectory(directory))
        {
            Logger.LogInformation("yarn add " + npmPackage.Name + versionPostfix);
            CmdHelper.RunCmd(NpmHelper.GetYarnCommand(directory, "add " + npmPackage.Name + versionPostfix));

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
        NpmHelper.EnsureSafePackageName(npmPackage.Name);

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
            CmdHelper.RunCmd(NpmHelper.GetYarnCommand(directory, "remove " + npmPackage.Name));

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
