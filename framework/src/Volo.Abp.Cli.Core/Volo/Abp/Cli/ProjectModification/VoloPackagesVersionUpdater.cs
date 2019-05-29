using System;
using System.IO;
using System.Text;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.ProjectModification
{
    public class VoloPackagesVersionUpdater : ITransientDependency
    {
        public void UpdateSolution(string solutionPath, string newVersion)
        {
            var projectPaths = ProjectFinder.GetProjectFiles(solutionPath);

            foreach (var filePath in projectPaths)
            {
                UpdateInternal(filePath, newVersion);
            }
        }

        public void UpdateProject(string projectPath, string newVersion)
        {
            UpdateInternal(projectPath, newVersion);
        }

        protected virtual void UpdateInternal(string projectPath, string newVersion)
        {
            var fileContent = File.ReadAllText(projectPath);

            var content = new StringBuilder();
            var index = 0;

            while (index >= 0)
            {
                fileContent = fileContent.Substring(index);
                content.Append(ReplaceAPackage(fileContent, newVersion, out index));
            }

            File.WriteAllText(projectPath, content.ToString());
        }

        private string ReplaceAPackage(string content, string version, out int index)
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

            returningText.Append(content.Substring(0, indexAfterQuote));
            content = content.Substring(indexAfterQuote);

            var indexAfterSecondQuote = content.IndexOf("\"", StringComparison.Ordinal) + 1;

            returningText.Append(content.Substring(0, indexAfterSecondQuote));
            content = content.Substring(indexAfterSecondQuote);

            var indexOfThirdQuote = content.IndexOf("\"", StringComparison.Ordinal);

            returningText.Append(version);

            index = indexOfPackageReference + packageReferenceStartText.Length + indexAfterQuote + indexAfterSecondQuote + indexOfThirdQuote;

            return returningText.ToString();
        }
    }
}
