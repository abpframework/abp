using NuGet.Versioning;
using System.IO;
using System.Xml;
using Volo.Abp.Cli.NuGet;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.Cli.ProjectModification
{
    public class VoloNugetPackagesVersionUpdater : ITransientDependency
    {
        private readonly NuGetService _nuGetService;

        public VoloNugetPackagesVersionUpdater(NuGetService nuGetService)
        {
            _nuGetService = nuGetService;
        }

        public void UpdateSolution(string solutionPath, bool includePreviews)
        {
            var projectPaths = ProjectFinder.GetProjectFiles(solutionPath);

            foreach (var filePath in projectPaths)
            {
                UpdateInternal(filePath, includePreviews);
            }
        }

        public void UpdateProject(string projectPath, bool includePreviews)
        {
            UpdateInternal(projectPath, includePreviews);
        }

        protected virtual void UpdateInternal(string projectPath, bool includePreviews)
        {
            var fileContent = File.ReadAllText(projectPath);

            File.WriteAllText(projectPath, UpdateVoloPackages(fileContent, includePreviews));
        }

        private string UpdateVoloPackages(string content, bool includePreviews)
        {
            var doc = new XmlDocument() { PreserveWhitespace = true };
            doc.LoadXml(content);

            foreach (XmlNode package in doc.SelectNodes("/Project/ItemGroup/PackageReference[starts-with(@Include, 'Volo.')]"))
            {
                var versionAttribute = package.Attributes["Version"];

                var packageId = package.Attributes["Include"].Value;
                var packageVersion = SemanticVersion.Parse(versionAttribute.Value);
                var latestVersion = AsyncHelper.RunSync(() => _nuGetService.GetLatestVersionOrNullAsync(packageId, includePreviews));

                if (latestVersion != null && packageVersion < latestVersion)
                {
                    versionAttribute.Value = latestVersion.ToString();
                }
            }

            return doc.OuterXml;
        }
    }
}
