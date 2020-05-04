using System;
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

        public async Task UpdateSolutionAsync(string solutionPath, bool includePreviews = false, bool switchToStable = false)
        {
            var projectPaths = ProjectFinder.GetProjectFiles(solutionPath);

            foreach (var filePath in projectPaths)
            {
                await UpdateInternalAsync(filePath, includePreviews, switchToStable);
            }
        }

        public async Task UpdateProjectAsync(string projectPath, bool includePreviews = false, bool switchToStable = false)
        {
            await UpdateInternalAsync(projectPath, includePreviews, switchToStable);
        }

        protected virtual async Task UpdateInternalAsync(string projectPath, bool includePreviews = false, bool switchToStable = false)
        {
            var fileContent = File.ReadAllText(projectPath);
            var updatedContent = await UpdateVoloPackagesAsync(fileContent, includePreviews, switchToStable);

            File.WriteAllText(projectPath, updatedContent);
        }

        private async Task<string> UpdateVoloPackagesAsync(string content, bool includePreviews = false, bool switchToStable = false)
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

                        if (includePreviews || (currentVersion.Contains("-preview") && !switchToStable))
                        {
                            var latestVersion = await GetLatestVersionFromMyGet(packageId);

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
                            var latestVersion = await _nuGetService.GetLatestVersionOrNullAsync(packageId);

                            if (latestVersion != null && (currentVersion.Contains("-preview") || currentSemanticVersion < latestVersion))
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

                    return await Task.FromResult(doc.OuterXml);
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
