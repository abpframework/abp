using System;
using System.Collections.Generic;
using NuGet.Versioning;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Volo.Abp.Cli.NuGet;
using Volo.Abp.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Volo.Abp.Cli.ProjectModification
{
    public class VoloNugetPackagesVersionUpdater : ITransientDependency
    {
        private readonly NuGetService _nuGetService;
        private readonly MyGetPackageListFinder _myGetPackageListFinder;
        public ILogger<VoloNugetPackagesVersionUpdater> Logger { get; set; }

        public VoloNugetPackagesVersionUpdater(NuGetService nuGetService, MyGetPackageListFinder myGetPackageListFinder)
        {
            _nuGetService = nuGetService;
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
                var latestVersionFromNuget = await _nuGetService.GetLatestVersionOrNullAsync("Volo.Abp.Core", includeReleaseCandidates : includeReleaseCandidates);
                var latestReleaseCandidateVersionFromNuget = await _nuGetService.GetLatestVersionOrNullAsync("Volo.Abp.Core", includeReleaseCandidates : true);
                var latestVersionFromMyGet = await GetLatestVersionFromMyGet("Volo.Abp.Core");

                async Task UpdateAsync(string filePath)
                {
                    var fileContent = File.ReadAllText(filePath);
                    var updatedContent = await UpdateVoloPackagesAsync(fileContent,
                        includePreviews,
                        includeReleaseCandidates,
                        switchToStable,
                        latestVersionFromNuget,
                        latestReleaseCandidateVersionFromNuget,
                        latestVersionFromMyGet,
                        version);

                    File.WriteAllText(filePath, updatedContent);
                }

                Task.WaitAll(projectPaths.Select(UpdateAsync).ToArray());
            }
        }

        public async Task UpdateProjectAsync(string projectPath, bool includeNightlyPreviews = false, bool includeReleaseCandidates = false, bool switchToStable = false, bool checkAll = false, string version = null)
        {
            if (checkAll && version.IsNullOrWhiteSpace())
            {
                await UpdateInternalAsync(projectPath, includeNightlyPreviews, includeReleaseCandidates,  switchToStable);
            }
            else
            {
                var latestVersionFromNuget = await _nuGetService.GetLatestVersionOrNullAsync("Volo.Abp.Core");
                var latestReleaseCandidateVersionFromNuget = await _nuGetService.GetLatestVersionOrNullAsync("Volo.Abp.Core", includeReleaseCandidates : true);
                var latestVersionFromMyGet = await GetLatestVersionFromMyGet("Volo.Abp.Core");

                var fileContent = File.ReadAllText(projectPath);

                var updatedContent = await UpdateVoloPackagesAsync(fileContent,
                    includeNightlyPreviews,
                    includeReleaseCandidates,
                    switchToStable,
                    latestVersionFromNuget,
                    latestReleaseCandidateVersionFromNuget,
                    latestVersionFromMyGet,
                    version);

                File.WriteAllText(projectPath, updatedContent);
            }
        }

        protected virtual async Task UpdateInternalAsync(string projectPath, bool includeNightlyPreviews = false, bool includeReleaseCandidates = false, bool switchToStable = false)
        {
            var fileContent = File.ReadAllText(projectPath);
            var updatedContent = await UpdateVoloPackagesAsync(fileContent, includeNightlyPreviews, includeReleaseCandidates, switchToStable);

            File.WriteAllText(projectPath, updatedContent);
        }

        protected virtual async Task<bool> SpecifiedVersionExists(string version, string packageId)
        {
            var versionList = await _nuGetService.GetPackageVersionListAsync(packageId);

            if (versionList.All(v => !v.Equals(version, StringComparison.OrdinalIgnoreCase)))
            {
                versionList = await _nuGetService.GetPackageVersionListAsync(packageId, true);
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
                        var currentVersion = versionAttribute.Value;
                        var currentSemanticVersion = SemanticVersion.Parse(currentVersion);

                        Logger.LogDebug("Checking package: \"{0}\" - Current version: {1}", packageId, currentSemanticVersion);

                        if (!specifiedVersion.IsNullOrWhiteSpace())
                        {
                            if (await SpecifiedVersionExists(specifiedVersion, packageId))
                            {
                                var specifiedSemanticVersion = SemanticVersion.Parse(specifiedVersion);
                                if (specifiedSemanticVersion > currentSemanticVersion)
                                {
                                    Logger.LogInformation("Updating package \"{0}\" from v{1} to v{2}.", packageId, currentVersion, specifiedVersion);
                                    versionAttribute.Value = specifiedVersion;
                                }
                                else
                                {
                                    Logger.LogWarning("Unable to update package \"{0}\" version v{1} to v{2}.",  packageId, currentVersion, specifiedVersion);
                                }
                            }
                            else
                            {
                                Logger.LogWarning("Package \"{0}\" specified version v{1} does not exist.", packageId, specifiedVersion);
                            }
                        }
                        else
                        {
                            if ((includeNightlyPreviews || (currentVersion.Contains("-preview") && !switchToStable)) && !includeReleaseCandidates)
                            {
                                var latestVersion = latestMyGetVersion ?? await GetLatestVersionFromMyGet(packageId);

                                if (currentVersion != latestVersion)
                                {
                                    Logger.LogInformation("Updating package \"{0}\" from v{1} to v{2}.", packageId, currentVersion, latestVersion);
                                    versionAttribute.Value = latestVersion;
                                }
                                else
                                {
                                    Logger.LogDebug("Package: \"{0}-v{1}\" is up to date.", packageId, currentVersion);
                                }
                            }
                            else
                            {
                                SemanticVersion latestVersion;
                                if (currentSemanticVersion.IsPrerelease && !switchToStable)
                                {
                                    latestVersion = latestNugetReleaseCandidateVersion ?? await _nuGetService.GetLatestVersionOrNullAsync(packageId, includeReleaseCandidates: true);
                                }
                                else
                                {
                                    latestVersion = latestNugetVersion ?? await _nuGetService.GetLatestVersionOrNullAsync(packageId, includeReleaseCandidates: includeReleaseCandidates);
                                }

                                if (latestVersion != null && (currentSemanticVersion < latestVersion || (currentSemanticVersion.IsPrerelease && switchToStable)))
                                {
                                    Logger.LogInformation("Updating package \"{0}\" from v{1} to v{2}.", packageId, currentSemanticVersion.ToString(), latestVersion.ToString());
                                    versionAttribute.Value = latestVersion.ToString();
                                }
                                else
                                {
                                    Logger.LogInformation("Package: \"{0}-v{1}\" is up to date.", packageId, currentSemanticVersion);
                                }
                            }
                        }
                    }

                    return doc.OuterXml;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Cannot update Volo.* packages! An error occured while updating the package \"{0}\". Error: {1}", packageId, ex.Message);
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
}
