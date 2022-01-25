using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Versioning;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.Commands;
using Volo.Abp.Cli.Http;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;
using Volo.Abp.IO;
using Volo.Abp.Threading;

namespace Volo.Abp.Cli.ProjectModification;

public class NpmPackagesUpdater : ITransientDependency
{
    public ILogger<NpmPackagesUpdater> Logger { get; set; }
    protected ICancellationTokenProvider CancellationTokenProvider { get; }
    public InstallLibsCommand InstallLibsCommand { get; }
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
        InstallLibsCommand ınstallLibsCommand,
        ICmdHelper cmdHelper)
    {
        _packageJsonFileFinder = packageJsonFileFinder;
        _npmGlobalPackagesChecker = npmGlobalPackagesChecker;
        CancellationTokenProvider = cancellationTokenProvider;
        InstallLibsCommand = ınstallLibsCommand;
        CmdHelper = cmdHelper;
        _cliHttpClientFactory = cliHttpClientFactory;
        Logger = NullLogger<NpmPackagesUpdater>.Instance;
    }

    public async Task Update(string rootDirectory, bool includePreviews = false,
        bool includeReleaseCandidates = false,
        bool switchToStable = false, string version = null)
    {
        var fileList = _packageJsonFileFinder.Find(rootDirectory);

        if (!fileList.Any())
        {
            return;
        }

        _npmGlobalPackagesChecker.Check();

        var packagesUpdated = new ConcurrentDictionary<string, bool>();

        async Task UpdateAsync(string file)
        {
            var updated = await UpdatePackagesInFile(file, includePreviews, includeReleaseCandidates,
                switchToStable,
                version);
            packagesUpdated.TryAdd(file, updated);
        }

        Task.WaitAll(fileList.Select(UpdateAsync).ToArray());

        foreach (var file in packagesUpdated.Where(x => x.Value))
        {
            var fileDirectory = Path.GetDirectoryName(file.Key).EnsureEndsWith(Path.DirectorySeparatorChar);

            if (includePreviews)
            {
                await CreateNpmrcFileAsync(Path.GetDirectoryName(file.Key));
            }
            else if (switchToStable)
            {
                await DeleteNpmrcFileAsync(Path.GetDirectoryName(file.Key));
            }

            if (await NpmrcFileExistAsync(fileDirectory))
            {
                RunNpmInstall(fileDirectory);
            }
            else
            {
                RunYarn(fileDirectory);
            }

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

        if (await NpmrcFileExistAsync(directoryName))
        {
            var fileContent = File.ReadAllText(fileName);

            if (!fileContent.Contains(abpRegistry))
            {
                fileContent += Environment.NewLine + abpRegistry;
            }

            File.WriteAllText(fileName, fileContent);

            return;
        }

        using var fs = File.Create(fileName);

        var content = new UTF8Encoding(true)
            .GetBytes(abpRegistry);
        fs.Write(content, 0, content.Length);
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
        string specifiedVersion = null)
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
                switchToStable, specifiedVersion);

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
        string specifiedVersion = null)
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
            if ((includePreviews ||
                 (!switchToStable && (currentVersion != null && currentVersion.Contains("-preview")))) &&
                !includeReleaseCandidates)
            {
                version = "preview";
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

    protected virtual async Task<string> GetLatestVersion(JProperty package, bool includeReleaseCandidates = false)
    {
        if (_fileVersionStorage.ContainsKey(package.Name))
        {
            return await Task.FromResult(_fileVersionStorage[package.Name]);
        }

        var versionList = GetPackageVersionList(package);

        var newVersion = includeReleaseCandidates
            ? versionList.First()
            : versionList.FirstOrDefault(v => !SemanticVersion.Parse(v).IsPrerelease);

        if (string.IsNullOrEmpty(newVersion))
        {
            _fileVersionStorage[package.Name] = newVersion;
            return await Task.FromResult(newVersion);
        }

        var newVersionWithPrefix = $"~{newVersion}";

        _fileVersionStorage[package.Name] = newVersionWithPrefix;

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
                .AddRange(properties.Where(p => p.Name.StartsWith("@abp/") || p.Name.StartsWith("@volo/"))
                    .ToList());
        }

        return abpPackages;
    }

    protected virtual async Task RunInstallLibsAsync(string fileDirectory)
    {
        var args = new CommandLineArgs("install-libs");
        args.Options.Add(InstallLibsCommand.Options.WorkingDirectory.Short, fileDirectory);

        await InstallLibsCommand.ExecuteAsync(args);
    }

    protected virtual void RunYarn(string fileDirectory)
    {
        Logger.LogInformation($"Running Yarn on {fileDirectory}");
        CmdHelper.RunCmd($"cd {fileDirectory} && yarn");
    }

    protected virtual void RunNpmInstall(string fileDirectory)
    {
        Logger.LogInformation($"Running npm install on {fileDirectory}");
        CmdHelper.RunCmd($"cd {fileDirectory} && npm install");
    }

    protected virtual List<string> GetPackageVersionList(JProperty package)
    {
        var output = CmdHelper.RunCmdAndGetOutput($"npm show {package.Name} versions --json");

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
