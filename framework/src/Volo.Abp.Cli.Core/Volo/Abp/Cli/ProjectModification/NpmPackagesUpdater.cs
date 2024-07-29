using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Versioning;
using Volo.Abp.Cli.Http;
using Volo.Abp.Cli.LIbs;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;
using Volo.Abp.IO;
using Volo.Abp.Threading;

namespace Volo.Abp.Cli.ProjectModification;

public class NpmPackagesUpdater : ITransientDependency
{
    public ILogger<NpmPackagesUpdater> Logger { get; set; }
    protected ICancellationTokenProvider CancellationTokenProvider { get; }
    public IInstallLibsService InstallLibsService { get; }
    public ICmdHelper CmdHelper { get; }

    private readonly PackageJsonFileFinder _packageJsonFileFinder;
    private readonly NpmGlobalPackagesChecker _npmGlobalPackagesChecker;
    private readonly Dictionary<string, string> _fileVersionStorage = new Dictionary<string, string>();
    private readonly CliHttpClientFactory _cliHttpClientFactory;

    public NpmPackagesUpdater(
        PackageJsonFileFinder packageJsonFileFinder,
        NpmGlobalPackagesChecker npmGlobalPackagesChecker,
        ICancellationTokenProvider cancellationTokenProvider,
        CliHttpClientFactory cliHttpClientFactory,
        IInstallLibsService installLibsService,
        ICmdHelper cmdHelper)
    {
        _packageJsonFileFinder = packageJsonFileFinder;
        _npmGlobalPackagesChecker = npmGlobalPackagesChecker;
        CancellationTokenProvider = cancellationTokenProvider;
        InstallLibsService = installLibsService;
        CmdHelper = cmdHelper;
        _cliHttpClientFactory = cliHttpClientFactory;
        Logger = NullLogger<NpmPackagesUpdater>.Instance;
    }

    public async Task Update(string rootDirectory, bool includePreviews = false,
        bool includeReleaseCandidates = false,
        bool switchToStable = false, string version = null, bool includePreRc = false)
    {
        var fileList = _packageJsonFileFinder.Find(rootDirectory);

        if (!fileList.Any())
        {
            return;
        }

        _npmGlobalPackagesChecker.Check();

        foreach (var file in fileList)
        {
            if (includePreviews || includePreRc)
            {
                await CreateNpmrcFileAsync(Path.GetDirectoryName(file));
            }
            else if (switchToStable)
            {
                await DeleteNpmrcFileAsync(Path.GetDirectoryName(file));
            }
        }

        var packagesUpdated = new ConcurrentDictionary<string, bool>();

        async Task UpdateAsync(string file)
        {
            var updated = await UpdatePackagesInFile(file, includePreviews, includeReleaseCandidates,
                switchToStable,
                version,
                includePreRc);

            packagesUpdated.TryAdd(file, updated);
        }

        Task.WaitAll(fileList.Select(UpdateAsync).ToArray());

        foreach (var file in packagesUpdated.Where(x => x.Value))
        {
            var fileDirectory = Path.GetDirectoryName(file.Key).EnsureEndsWith(Path.DirectorySeparatorChar);

            RunYarn(fileDirectory);

            if (!IsAngularProject(fileDirectory))
            {
                Thread.Sleep(1000);
                RunInstallLibsAsync(fileDirectory);
            }
        }
    }

    private static async Task DeleteNpmrcFileAsync(string directoryName)
    {
        FileHelper.DeleteIfExists(Path.Combine(directoryName, ".npmrc"));

        await Task.CompletedTask;
    }

    private static async Task<bool> NpmrcFileExistAsync(string directoryName)
    {
        return await Task.FromResult(File.Exists(Path.Combine(directoryName, ".npmrc")));
    }

    private async Task CreateNpmrcFileAsync(string directoryName)
    {
        var fileName = Path.Combine(directoryName, ".npmrc");
        var abpRegistry = "@abp:registry=https://www.myget.org/F/abp-nightly/npm";
        var voloRegistry = "@volo:registry=https://www.myget.org/F/abp-commercial-npm-nightly/npm";
        var volosoftRegistry = "@volosoft:registry=https://www.myget.org/F/abp-commercial-npm-nightly/npm";

        if (await NpmrcFileExistAsync(directoryName))
        {
            var fileContent = File.ReadAllText(fileName);

            if (!fileContent.Contains(abpRegistry))
            {
                fileContent += Environment.NewLine + abpRegistry;
            }

            if (!fileContent.Contains(voloRegistry))
            {
                fileContent += Environment.NewLine + voloRegistry;
            }

            if (!fileContent.Contains(volosoftRegistry))
            {
                fileContent += volosoftRegistry;
            }

            File.WriteAllText(fileName, fileContent);

            return;
        }

        using var sw = File.CreateText(fileName);

        sw.WriteLine(abpRegistry);
        sw.WriteLine(voloRegistry);
        sw.WriteLine(volosoftRegistry);
    }

    private static bool IsAngularProject(string fileDirectory)
    {
        return File.Exists(Path.Combine(fileDirectory, "angular.json"));
    }

    protected virtual async Task<bool> UpdatePackagesInFile(
        string filePath,
        bool includePreviews = false,
        bool includeReleaseCandidates = false,
        bool switchToStable = false,
        string specifiedVersion = null,
        bool includePreRc = false)
    {
        var packagesUpdated = false;
        var fileContent = File.ReadAllText(filePath);
        var packageJson = JObject.Parse(fileContent);
        var abpPackages = GetAbpPackagesFromPackageJson(packageJson);

        if (!abpPackages.Any())
        {
            return false;
        }

        foreach (var abpPackage in abpPackages)
        {
            var updated = await TryUpdatingPackage(filePath, abpPackage, includePreviews, includeReleaseCandidates,
                switchToStable, specifiedVersion, includePreRc);

            if (updated)
            {
                packagesUpdated = true;
            }
        }

        var updatedContent = packageJson.ToString(Formatting.Indented);

        File.WriteAllText(filePath, updatedContent);

        return packagesUpdated;
    }

    protected virtual async Task<bool> TryUpdatingPackage(
        string filePath,
        JProperty package,
        bool includePreviews = false,
        bool includeReleaseCandidates = false,
        bool switchToStable = false,
        string specifiedVersion = null,
        bool includePreRc = false)
    {
        var currentVersion = (string)package.Value;

        var version = string.Empty;

        if (!specifiedVersion.IsNullOrWhiteSpace())
        {
            if (!SpecifiedVersionExists(specifiedVersion, package))
            {
                return false;
            }

            if (SemanticVersion.Parse(specifiedVersion) <=
                SemanticVersion.Parse(currentVersion.RemovePreFix("~", "^")))
            {
                return false;
            }

            version = specifiedVersion.EnsureStartsWith('^');
        }
        else
        {
            if (includePreRc && !includeReleaseCandidates)
            {
                version = await GetLatestVersion(package, includePreRc: true, workingDirectory: filePath.RemovePostFix("package.json"));
            }
            else if ((includePreviews ||
                 (!switchToStable && (currentVersion != null && currentVersion.Contains("-preview")))) &&
                !includeReleaseCandidates)
            {
                version = await GetLatestVersion(package, includePreviews: includePreviews, workingDirectory: filePath.RemovePostFix("package.json"));
            }
            else
            {
                if (!switchToStable && IsPrerelease(currentVersion))
                {
                    version = await GetLatestVersion(package, true);
                }
                else
                {
                    version = await GetLatestVersion(package, includeReleaseCandidates);
                }
            }
        }

        if (string.IsNullOrEmpty(version) || version == currentVersion)
        {
            return false;
        }

        var prefix = string.Empty;
        if (package.Value.ToString().StartsWith("~"))
        {
            prefix = "~";
        }
        else if (package.Value.ToString().StartsWith("^"))
        {
            prefix = "^";
        }

        version = prefix + version.RemovePreFix("~", "^");
        package.Value.Replace(version);

        Logger.LogInformation(
            $"Updated {package.Name} to {version} in {filePath.Replace(Directory.GetCurrentDirectory(), "")}.");
        return true;
    }

    protected virtual bool IsPrerelease(string version)
    {
        if (version == null)
        {
            return false;
        }

        return version.Split("-", StringSplitOptions.RemoveEmptyEntries).Length > 1;
    }

    protected virtual async Task<string> GetLatestVersion(JProperty package, bool includeReleaseCandidates = false, bool includePreviews = false, string workingDirectory = null, bool includePreRc = false)
    {
        var postfix = includePreviews || includePreRc ? "(preview)" : string.Empty;
        var key = package.Name + postfix;

        if (_fileVersionStorage.ContainsKey(key))
        {
            return await Task.FromResult(_fileVersionStorage[key]);
        }

        var versionList = GetPackageVersionList(package, workingDirectory);

        string newVersion = string.Empty;

        if (includePreRc)
        {
            var filterKey = $"-preview{DateTime.Now.ToString("yyyyMMdd")}";
            newVersion = versionList.Where(f => f.Contains(filterKey)).OrderBy(o => o).FirstOrDefault();
        }
        else if (includePreviews)
        {
            newVersion = versionList.FirstOrDefault(v => v.Contains("-preview"));
        }
        else
        {
            newVersion = includeReleaseCandidates
                ? versionList.First()
                : versionList.FirstOrDefault(v => !SemanticVersion.Parse(v).IsPrerelease);
        }

        if (string.IsNullOrEmpty(newVersion))
        {
            _fileVersionStorage[key] = newVersion;
            return await Task.FromResult(newVersion);
        }

        var newVersionWithPrefix = $"~{newVersion}";

        _fileVersionStorage[key] = newVersionWithPrefix;

        return await Task.FromResult(newVersionWithPrefix);
    }

    protected virtual List<JProperty> GetAbpPackagesFromPackageJson(JObject fileObject)
    {
        var dependencyList = new[] { "dependencies", "devDependencies", "peerDependencies" };
        var abpPackages = new List<JProperty>();

        foreach (var dependencyListName in dependencyList)
        {
            var dependencies = (JObject)fileObject[dependencyListName];

            if (dependencies == null)
            {
                continue;
            }

            var properties = dependencies.Properties().ToList();

            abpPackages
                .AddRange(
                properties.Where(
                      p => p.Name.StartsWith("@abp/")
                        || p.Name.StartsWith("@volo/")
                        || p.Name.StartsWith("@volosoft/")).ToList()
                );
        }

        return abpPackages;
    }

    protected virtual async Task RunInstallLibsAsync(string fileDirectory)
    {
        Logger.LogInformation("Installing client-side packages...");
        await InstallLibsService.InstallLibsAsync(fileDirectory);
    }

    protected virtual void RunYarn(string fileDirectory)
    {
        Logger.LogInformation($"Running Yarn on {fileDirectory}");
        CmdHelper.RunCmd($"yarn", fileDirectory);
    }

    protected virtual void RunNpmInstall(string fileDirectory)
    {
        Logger.LogInformation($"Running npm install on {fileDirectory}");
        CmdHelper.RunCmd($"npm install", fileDirectory);
    }

    protected virtual List<string> GetPackageVersionList(JProperty package, string workingDirectory = null)
    {
        var output = CmdHelper.RunCmdAndGetOutput($"npm show {package.Name} versions --json", workingDirectory);

        var versionListAsJson = ExtractVersions(output);

        return JsonConvert.DeserializeObject<string[]>(versionListAsJson)
            .OrderByDescending(SemanticVersion.Parse, new VersionComparer()).ToList();
    }

    protected virtual string ExtractVersions(string output)
    {
        var arrayStart = output.IndexOf('[');
        return output.Substring(arrayStart, output.IndexOf(']') - arrayStart + 1);
    }

    protected virtual bool SpecifiedVersionExists(string version, JProperty package)
    {
        var versionList = GetPackageVersionList(package);

        return versionList.Any(v => v.Equals(version, StringComparison.OrdinalIgnoreCase));
    }
}
