using System;
using NuGet.Versioning;
using System.IO;
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
        public ILogger<VoloNugetPackagesVersionUpdater> Logger { get; set; }

        public VoloNugetPackagesVersionUpdater(NuGetService nuGetService)
        {
            _nuGetService = nuGetService;
            Logger = NullLogger<VoloNugetPackagesVersionUpdater>.Instance;
        }

        public async Task UpdateSolutionAsync(string solutionPath, bool includePreviews)
        {
            var projectPaths = ProjectFinder.GetProjectFiles(solutionPath);

            foreach (var filePath in projectPaths)
            {
                await UpdateInternalAsync(filePath, includePreviews);
            }
        }

        public async Task UpdateProjectAsync(string projectPath, bool includePreviews)
        {
            await UpdateInternalAsync(projectPath, includePreviews);
        }

        protected virtual async Task UpdateInternalAsync(string projectPath, bool includePreviews)
        {
            var fileContent = File.ReadAllText(projectPath);

            File.WriteAllText(projectPath, await UpdateVoloPackagesAsync(fileContent, includePreviews));
        }

        private async Task<string> UpdateVoloPackagesAsync(string content, bool includePreviews)
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

                        var versionAttribute = package.Attributes["Version"];

                        packageId = package.Attributes["Include"].Value;
                        var packageVersion = SemanticVersion.Parse(versionAttribute.Value);

                        Logger.LogDebug("Checking package: \"{0}\" - Current version: {1}", packageId, packageVersion);

                        var latestVersion = await _nuGetService.GetLatestVersionOrNullAsync(packageId, includePreviews);

                        if (latestVersion != null && packageVersion < latestVersion)
                        {
                            Logger.LogInformation("Updating package \"{0}\" from v{1} to v{2}.", packageId, packageVersion.ToString(), latestVersion.ToString());
                            versionAttribute.Value = latestVersion.ToString();
                        }
                        else
                        {
                            Logger.LogDebug("Package: \"{0}-v{1}\" is up to date.", packageId, packageVersion);
                        }
                    }

                    return await Task.FromResult(doc.OuterXml);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Cannot update volo packages! An error occured while updating the package \"{0}\". Error: {1}", packageId, ex.Message);
                Logger.LogException(ex);
            }

            return await Task.FromResult(content);
        }
    }
}
