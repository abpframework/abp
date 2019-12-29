using NuGet.Versioning;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using Volo.Abp.Cli.NuGet;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.ProjectModification
{
    public class VoloNugetPackagesVersionUpdater : ITransientDependency
    {
        private readonly NuGetService _nuGetService;

        public VoloNugetPackagesVersionUpdater(NuGetService nuGetService)
        {
            _nuGetService = nuGetService;
        }

        public async Task UpdateSolutionAsync(string solutionPath, bool includePreviews)
        {
            var projectPaths = ProjectFinder.GetProjectFiles(solutionPath);

            foreach (var filePath in projectPaths)
            {
                await UpdateInternalAsync(filePath, includePreviews).ConfigureAwait(false);
            }
        }

        public async Task UpdateProjectAsync(string projectPath, bool includePreviews)
        {
            await UpdateInternalAsync(projectPath, includePreviews).ConfigureAwait(false);
        }

        protected virtual async Task UpdateInternalAsync(string projectPath, bool includePreviews)
        {
            var fileContent = File.ReadAllText(projectPath);

            File.WriteAllText(projectPath, await UpdateVoloPackagesAsync(fileContent, includePreviews).ConfigureAwait(false));
        }

        private async Task<string> UpdateVoloPackagesAsync(string content, bool includePreviews)
        {
            var doc = new XmlDocument() { PreserveWhitespace = true };
            doc.LoadXml(content);

            foreach (XmlNode package in doc.SelectNodes("/Project/ItemGroup/PackageReference[starts-with(@Include, 'Volo.')]"))
            {
                var versionAttribute = package.Attributes["Version"];

                var packageId = package.Attributes["Include"].Value;
                var packageVersion = SemanticVersion.Parse(versionAttribute.Value);
                var latestVersion = await _nuGetService.GetLatestVersionOrNullAsync(packageId, includePreviews).ConfigureAwait(false);

                if (latestVersion != null && packageVersion < latestVersion)
                {
                    versionAttribute.Value = latestVersion.ToString();
                }
            }

            return doc.OuterXml;
        }
    }
}
