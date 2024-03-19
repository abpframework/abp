using System;
using System.Collections.Generic;
using NuGet.Versioning;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Volo.Abp.Cli.Version;
using Volo.Abp.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Text;

namespace Volo.Abp.Cli.ProjectModification;

public class VoloNugetPackagesVersionUpdater : ITransientDependency
{
    private readonly PackageVersionCheckerService _packageVersionCheckerService;
    private readonly MyGetPackageListFinder _myGetPackageListFinder;
    public ILogger<VoloNugetPackagesVersionUpdater> Logger { get; set; }
    public static Encoding DefaultEncoding = Encoding.UTF8;

    public VoloNugetPackagesVersionUpdater(PackageVersionCheckerService packageVersionCheckerService, MyGetPackageListFinder myGetPackageListFinder)
    {
        _packageVersionCheckerService = packageVersionCheckerService;
        _myGetPackageListFinder = myGetPackageListFinder;
        Logger = NullLogger<VoloNugetPackagesVersionUpdater>.Instance;
    }

    public async Task UpdateSolutionAsync(string solutionPath, bool includePreviews = false, bool includeReleaseCandidates = false, bool switchToStable = false, bool checkAll = false, string version = null)
    {
        var projectPaths = ProjectFinder.GetProjectFiles(solutionPath);

        if (checkAll && version.IsNullOrWhiteSpace())
        {
            Task.WaitAll(projectPaths.Select(projectPath => UpdateInternalAsync(projectPath, includePreviews, includeReleaseCandidates, switchToStable)).ToArray());
        }
        else
        {
            var latestVersionInfo = await _packageVersionCheckerService.GetLatestVersionOrNullAsync("Volo.Abp.Core", includeReleaseCandidates: includeReleaseCandidates);
            var latestReleaseCandidateVersionInfo = await _packageVersionCheckerService.GetLatestVersionOrNullAsync("Volo.Abp.Core", includeReleaseCandidates: true);
            var latestVersionFromMyGet = await GetLatestVersionFromMyGet("Volo.Abp.Core");

            async Task UpdateAsync(string filePath)
            {
                using (var fs = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    using (var sr = new StreamReader(fs, Encoding.Default, true))
                    {
                        var fileContent = await sr.ReadToEndAsync();

                        var updatedContent = await UpdateVoloPackagesAsync(fileContent,
                            includePreviews,
                            includeReleaseCandidates,
                            switchToStable,
                            latestVersionInfo.Version,
                            latestReleaseCandidateVersionInfo.Version,
                            latestVersionFromMyGet,
                            version);

                        fs.Seek(0, SeekOrigin.Begin);
                        fs.SetLength(0);
                        using (var sw = new StreamWriter(fs, DefaultEncoding))
                        {
                            await sw.WriteAsync(updatedContent);
                            await sw.FlushAsync();
                        }
                    }
                }
            }

            Task.WaitAll(projectPaths.Select(UpdateAsync).ToArray());
        }
    }

    public async Task UpdateProjectAsync(string projectPath, bool includeNightlyPreviews = false, bool includeReleaseCandidates = false, bool switchToStable = false, bool checkAll = false, string version = null)
    {
        if (checkAll && version.IsNullOrWhiteSpace())
        {
            await UpdateInternalAsync(projectPath, includeNightlyPreviews, includeReleaseCandidates, switchToStable);
        }
        else
        {
            var latestVersionInfo = await _packageVersionCheckerService.GetLatestVersionOrNullAsync("Volo.Abp.Core");
            var latestReleaseCandidateVersionInfo = await _packageVersionCheckerService.GetLatestVersionOrNullAsync("Volo.Abp.Core", includeReleaseCandidates: true);
            var latestVersionFromMyGet = await GetLatestVersionFromMyGet("Volo.Abp.Core");

            using (var fs = File.Open(projectPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                using (var sr = new StreamReader(fs, Encoding.Default, true))
                {
                    var fileContent = await sr.ReadToEndAsync();

                    var updatedContent = await UpdateVoloPackagesAsync(fileContent,
                        includeNightlyPreviews,
                        includeReleaseCandidates,
                        switchToStable,
                        latestVersionInfo.Version,
                        latestReleaseCandidateVersionInfo.Version,
                        latestVersionFromMyGet,
                        version);

                    fs.Seek(0, SeekOrigin.Begin);
                    fs.SetLength(0);

                    using (var sw = new StreamWriter(fs, sr.CurrentEncoding))
                    {
                        await sw.WriteAsync(updatedContent);
                        await sw.FlushAsync();
                    }
                }
            }
        }
    }

    protected virtual async Task UpdateInternalAsync(string projectPath, bool includeNightlyPreviews = false, bool includeReleaseCandidates = false, bool switchToStable = false)
    {
        using (var fs = File.Open(projectPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
        {
            using (var sr = new StreamReader(fs, Encoding.Default, true))
            {
                var fileContent = await sr.ReadToEndAsync();

                var updatedContent = await UpdateVoloPackagesAsync(fileContent, includeNightlyPreviews, includeReleaseCandidates, switchToStable);

                fs.Seek(0, SeekOrigin.Begin);
                fs.SetLength(0);

                using (var sw = new StreamWriter(fs, sr.CurrentEncoding))
                {
                    await sw.WriteAsync(updatedContent);
                    await sw.FlushAsync();
                }
            }
        }
    }

    protected virtual async Task<bool> SpecifiedVersionExists(string version, string packageId)
    {
        var versionList = await _packageVersionCheckerService.GetPackageVersionListAsync(packageId);

        if (versionList.All(v => !v.Equals(version, StringComparison.OrdinalIgnoreCase)))
        {
            versionList = await _packageVersionCheckerService.GetPackageVersionListAsync(packageId, true);
        }

        return versionList.Any(v => v.Equals(version, StringComparison.OrdinalIgnoreCase));
    }

    private async Task<string> UpdateVoloPackagesAsync(string content,
        bool includeNightlyPreviews = false,
        bool includeReleaseCandidates = false,
        bool switchToStable = false,
        SemanticVersion latestNugetVersion = null,
        SemanticVersion latestNugetReleaseCandidateVersion = null,
        string latestMyGetVersion = null,
        string specifiedVersion = null)
    {
        string packageId = null;

        try
        {
            var doc = new XmlDocument()
            {
                PreserveWhitespace = true
            };

            doc.LoadXml(content);

            var packageNodeList = doc.SelectNodes("/Project/ItemGroup/PackageReference[starts-with(@Include, 'Volo.')]");

            if (packageNodeList != null)
            {
                foreach (XmlNode package in packageNodeList)
                {
                    if (package.Attributes == null)
                    {
                        continue;
                    }

                    packageId = package.Attributes["Include"].Value;

                    var versionAttribute = package.Attributes["Version"];
                    if (versionAttribute == null)
                    {
                        Logger.LogWarning("Package: {PackageId} uses central package management. Skipped!", packageId);
                        continue;
                    }
                    var currentVersion = versionAttribute.Value;
                    
                    var isLeptonXPackage = packageId.Contains("LeptonX");
                    var isStudioPackage = packageId.StartsWith("Volo.Abp.Studio.");
                    if(isLeptonXPackage)
                    {
                        //'SemanticVersion.TryParse' can not parse the version if the version contains floating version resolution, such as '*-*'
                        currentVersion = currentVersion.Replace("*-*", "0").Replace("*", "0");
                    }

                    var isVersionParsed = SemanticVersion.TryParse(currentVersion, out var currentSemanticVersion);
                    if (!isVersionParsed)
                    {
                        Logger.LogWarning("Could not parse package \"{PackageId}\" version v{CurrentVersion}. Skipped!", packageId, currentVersion);
                        continue;
                    }

                    Logger.LogDebug("Checking package: \"{PackageId}\" - Current version: {CurrentSemanticVersion}", packageId, currentSemanticVersion);

                    if (!specifiedVersion.IsNullOrWhiteSpace())
                    {
                        if (isLeptonXPackage || isStudioPackage)
                        {
                            Logger.LogWarning("Package: {PackageId} could not be updated. Please manually update the package version yourself to prevent version mismatches!", packageId);
                            continue;
                        }

                        if (await SpecifiedVersionExists(specifiedVersion, packageId))
                        {
                            var specifiedSemanticVersion = SemanticVersion.Parse(specifiedVersion);
                            if (specifiedSemanticVersion > currentSemanticVersion)
                            {
                                Logger.LogInformation("Updating package \"{PackageId}\" from v{CurrentVersion} to v{SpecifiedVersion}", packageId, currentVersion, specifiedVersion);
                                versionAttribute.Value = specifiedVersion;
                            }
                            else
                            {
                                Logger.LogWarning("Unable to update package \"{PackageId}\" version v{CurrentVersion} to v{SpecifiedVersion}", packageId, currentVersion, specifiedVersion);
                            }
                        }
                        else
                        {
                            Logger.LogWarning("Package \"{PackageId}\" specified version v{SpecifiedVersion} does not exist!", packageId, specifiedVersion);
                        }
                    }
                    else
                    {
                        if ((includeNightlyPreviews || (currentVersion.Contains("-preview") && !switchToStable)) && !includeReleaseCandidates)
                        {
                            string latestVersion;
                            if(isLeptonXPackage)
                            {
                                var leptonXPackageName = packageId;
                                if(includeNightlyPreviews) 
                                {
                                    //use LeptonX Lite package as the package name to be able to get the package version from the 'abp-nightly' feed.
                                    leptonXPackageName = "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite";
                                }

                                latestVersion = (await _packageVersionCheckerService.GetLatestVersionOrNullAsync(leptonXPackageName, includeNightlyPreviews, includeReleaseCandidates))?.Version?.ToString();
                            }
                            else if(isStudioPackage)
                            {
                                latestVersion = (await _packageVersionCheckerService.GetLatestVersionOrNullAsync(packageId, includeNightlyPreviews, includeReleaseCandidates))?.Version?.ToString();
                            }
                            else
                            {
                                latestVersion = latestMyGetVersion == null ? await GetLatestVersionFromMyGet(packageId) : latestMyGetVersion;
                            }

                            if(latestVersion == null)
                            {
                                Logger.LogWarning("Package: {PackageId} could not be updated. Please manually update the package version yourself to prevent version mismatches!", packageId);
                                continue;
                            }

                            if (currentVersion != latestVersion)
                            {
                                Logger.LogInformation("Updating package \"{PackageId}\" from v{CurrentVersion} to v{LatestVersion}", packageId, currentVersion, latestVersion);
                                versionAttribute.Value = latestVersion;
                            }
                            else
                            {
                                Logger.LogDebug("Package: \"{PackageId}-v{CurrentVersion}\" is up to date", packageId, currentVersion);
                            }
                        }
                        else
                        {
                            SemanticVersion latestVersion;
                            if (currentSemanticVersion.IsPrerelease && !switchToStable)
                            {
                                latestVersion = latestNugetReleaseCandidateVersion == null || isLeptonXPackage || isStudioPackage
                                    ? (await _packageVersionCheckerService.GetLatestVersionOrNullAsync(packageId, includeReleaseCandidates: true))?.Version 
                                    : latestNugetReleaseCandidateVersion;
                            }
                            else
                            {
                                latestVersion = latestNugetVersion == null || isLeptonXPackage || isStudioPackage
                                    ? (await _packageVersionCheckerService.GetLatestVersionOrNullAsync(packageId, includeReleaseCandidates: includeReleaseCandidates))?.Version 
                                    : latestNugetVersion;
                            }

                            if (latestVersion != null && (currentSemanticVersion < latestVersion || (currentSemanticVersion.IsPrerelease && switchToStable)))
                            {
                                Logger.LogInformation("Updating package \"{PackageId}\" from v{CurrentSemanticVersion} to v{LatestVersion}", packageId, currentSemanticVersion.ToString(), latestVersion.ToString());
                                versionAttribute.Value = latestVersion.ToString();
                            }
                            else
                            {
                                Logger.LogInformation("Package: \"{PackageId}-v{CurrentSemanticVersion}\" is up to date", packageId, currentSemanticVersion);
                            }
                        }
                    }
                }

                return doc.OuterXml;
            }
        }
        catch (Exception ex)
        {
            Logger.LogError("Cannot update Volo.* packages! An error occurred while updating the package \"{PackageId}\". Error: {ErrorMessage}", packageId, ex.Message);
            Logger.LogException(ex);
        }

        return await Task.FromResult(content);
    }

    private async Task<string> GetLatestVersionFromMyGet(string packageId)
    {
        var myGetPack = await _myGetPackageListFinder.GetPackagesAsync();

        return myGetPack.Packages.FirstOrDefault(p => p.Id == packageId)?.Versions.LastOrDefault();
    }
}
