using System;
using System.IO;
using System.Text;
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

            var content = new StringBuilder();
            var index = 0;

            while (index >= 0)
            {
                fileContent = fileContent.Substring(index);
                content.Append(ReplaceAPackage(fileContent, includePreviews, out index));
            }

            File.WriteAllText(projectPath, content.ToString());
        }

        private string ReplaceAPackage(string content, bool includePreviews, out int index)
        {
            var packageReferenceStartText = "nclude=\"Volo.";
            var returningText = new StringBuilder();

            var indexOfPackageReference = content.IndexOf(packageReferenceStartText, StringComparison.Ordinal);

            if (indexOfPackageReference < 0)
            {
                index = -1;
                return content;
            }

            returningText.Append(content.Substring(0, indexOfPackageReference + packageReferenceStartText.Length));
            content = content.Substring(indexOfPackageReference + packageReferenceStartText.Length);

            var indexAfterQuote = content.IndexOf("\"", StringComparison.Ordinal) + 1;

            var packageId = "Volo." + content.Substring(indexAfterQuote - 1);

            returningText.Append(content.Substring(0, indexAfterQuote));
            content = content.Substring(indexAfterQuote);

            var indexAfterSecondQuote = content.IndexOf("\"", StringComparison.Ordinal) + 1;

            returningText.Append(content.Substring(0, indexAfterSecondQuote));
            content = content.Substring(indexAfterSecondQuote);

            var indexOfThirdQuote = content.IndexOf("\"", StringComparison.Ordinal);

            var version = _nuGetService.GetLatestVersionOrNullAsync(packageId, includePreviews).GetAwaiter().GetResult();
            returningText.Append(version.ToString());

            index = indexOfPackageReference + packageReferenceStartText.Length + indexAfterQuote + indexAfterSecondQuote + indexOfThirdQuote;

            return returningText.ToString();
        }
    }
}
