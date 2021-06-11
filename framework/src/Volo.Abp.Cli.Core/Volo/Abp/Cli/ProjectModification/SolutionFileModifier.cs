using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.ProjectModification
{
    public class SolutionFileModifier : ITransientDependency
    {
        public async Task RemoveProjectFromSolutionFileAsync(string solutionFile, string projectName)
        {
            var solutionFileContent = File.ReadAllText(solutionFile);
            solutionFileContent.NormalizeLineEndings();
            var lines = solutionFileContent.Split(new[] {Environment.NewLine, "\n"}, StringSplitOptions.None);
            File.WriteAllText(solutionFile,
                RemoveProject(lines.ToList(), projectName).JoinAsString(Environment.NewLine));
        }

        public async Task AddModuleToSolutionFileAsync(ModuleWithMastersInfo module, string solutionFile)
        {
            await AddModuleAsync(module, solutionFile);
        }

        public async Task AddPackageToSolutionFileAsync(NugetPackageInfo package, string solutionFile)
        {
            await AddPackageAsync(package, solutionFile);
        }

        private async Task AddPackageAsync(NugetPackageInfo package, string solutionFile)
        {
            var srcFolderId = await AddNewFolderAndGetIdOrGetExistingIdAsync(solutionFile, "src");

            var file = File.ReadAllText(solutionFile);
            var lines = file.Split(Environment.NewLine).ToList();

            if (lines.Any(l => l.Contains($"\"{package.Name}\"")))
            {
                return;
            }

            var projectGuid = Guid.NewGuid().ToString();

            var newProjectLine = "Project(\"{9A19103F-16F7-4668-BE54-9A1E7A4F7556}\") = \"" + package.Name + "\"," +
                                 " \"packages\\" + package.Name + "\\" +
                                 "\\" + package.Name + ".csproj\", \"{" + projectGuid + "}\""
                                 + Environment.NewLine + "EndProject";

            lines.InsertAfter(l => l.Trim().Equals("EndProject"), newProjectLine);

            var newPostSolutionLine =
                "		{" + projectGuid + "}.Debug|Any CPU.ActiveCfg = Debug|Any CPU" + Environment.NewLine +
                "		{" + projectGuid + "}.Debug|Any CPU.Build.0 = Debug|Any CPU" + Environment.NewLine +
                "		{" + projectGuid + "}.Release|Any CPU.ActiveCfg = Release|Any CPU" + Environment.NewLine +
                "		{" + projectGuid + "}.Release|Any CPU.Build.0 = Release|Any CPU";

            lines.InsertAfter(l => l.Contains("GlobalSection") && l.Contains("ProjectConfigurationPlatforms"),
                newPostSolutionLine);

            var newPreSolutionLine =
                "		{" + projectGuid + "} = {" + srcFolderId + "}";

            lines.InsertAfter(l => l.Contains("GlobalSection") && l.Contains("NestedProjects"), newPreSolutionLine);

            File.WriteAllText(solutionFile, string.Join(Environment.NewLine, lines));
        }

        private List<string> RemoveProject(List<string> solutionFileLines, string projectName)
        {
            var projectKey = FindProjectKey(solutionFileLines, projectName);

            if (projectKey == null)
            {
                return solutionFileLines;
            }

            var newSolutionFileLines = new List<string>();
            var firstOccurence = true;

            for (var i = 0; i < solutionFileLines.Count; ++i)
            {
                if (solutionFileLines[i].Contains(projectKey))
                {
                    if (firstOccurence)
                    {
                        firstOccurence = false;
                        ++i; //Skip "EndProject" line too.
                    }

                    continue;
                }

                newSolutionFileLines.Add(solutionFileLines[i]);
            }

            return newSolutionFileLines;
        }

        private string FindProjectKey(List<string> solutionFileLines, string projectName)
        {
            var projectNameWithQuotes = $"\"{projectName}\"";
            foreach (var solutionFileLine in solutionFileLines)
            {
                if (solutionFileLine.Contains(projectNameWithQuotes))
                {
                    var curlyBracketStartIndex = solutionFileLine.LastIndexOf("{", StringComparison.OrdinalIgnoreCase);
                    var curlyBracketEndIndex = solutionFileLine.LastIndexOf("}", StringComparison.OrdinalIgnoreCase);
                    return solutionFileLine.Substring(curlyBracketStartIndex + 1,
                        curlyBracketEndIndex - curlyBracketStartIndex - 1);
                }
            }

            return null;
        }

        private async Task AddModuleAsync(ModuleWithMastersInfo module, string solutionFile)
        {
            var srcModuleFolderId = await AddNewFolderAndGetIdOrGetExistingIdAsync(solutionFile, module.Name,
                await AddNewFolderAndGetIdOrGetExistingIdAsync(solutionFile, "modules"));
            var testModuleFolderId = await AddNewFolderAndGetIdOrGetExistingIdAsync(solutionFile, module.Name + ".Tests",
                await AddNewFolderAndGetIdOrGetExistingIdAsync(solutionFile, "test"));

            var file = File.ReadAllText(solutionFile);
            var lines = file.Split(Environment.NewLine).ToList();

            var projectsUnderModule = Directory.GetFiles(
                Path.Combine(Path.GetDirectoryName(solutionFile), "modules", module.Name),
                "*.csproj",
                SearchOption.AllDirectories);

            var projectsUnderTest = Directory.GetFiles(
                Path.Combine(Path.GetDirectoryName(solutionFile), "modules", module.Name, "test"),
                "*.csproj",
                SearchOption.AllDirectories);

            foreach (var projectPath in projectsUnderModule)
            {
                var parentFolderId = projectsUnderTest.Contains(projectPath) ? testModuleFolderId : srcModuleFolderId;
                var projectId = Path.GetFileName(projectPath).Replace(".csproj", "");
                var projectParentFolderInModule = projectsUnderTest.Contains(projectPath) ? "test" : "src";

                if (lines.Any(l => l.Contains($"\"{projectId}\"")))
                {
                    continue;
                }

                var projectGuid = Guid.NewGuid().ToString();

                var newProjectLine = "Project(\"{9A19103F-16F7-4668-BE54-9A1E7A4F7556}\") = \"" + projectId + "\"," +
                                     " \"modules\\" + module.Name + "\\" + projectParentFolderInModule + "\\" +
                                     projectId + "\\" + projectId + ".csproj\", \"{" + projectGuid + "}\""
                                     + Environment.NewLine + "EndProject";

                lines.InsertAfter(l => l.Trim().Equals("EndProject"), newProjectLine);

                var newPostSolutionLine =
                    "		{" + projectGuid + "}.Debug|Any CPU.ActiveCfg = Debug|Any CPU" + Environment.NewLine +
                    "		{" + projectGuid + "}.Debug|Any CPU.Build.0 = Debug|Any CPU" + Environment.NewLine +
                    "		{" + projectGuid + "}.Release|Any CPU.ActiveCfg = Release|Any CPU" + Environment.NewLine +
                    "		{" + projectGuid + "}.Release|Any CPU.Build.0 = Release|Any CPU";

                lines.InsertAfter(l => l.Contains("GlobalSection") && l.Contains("ProjectConfigurationPlatforms"),
                    newPostSolutionLine);

                var newPreSolutionLine =
                    "		{" + projectGuid + "} = {" + parentFolderId + "}";

                lines.InsertAfter(l => l.Contains("GlobalSection") && l.Contains("NestedProjects"), newPreSolutionLine);
            }

            File.WriteAllText(solutionFile, string.Join(Environment.NewLine, lines));

            if (module.MasterModuleInfos != null)
            {
                foreach (var masterModule in module.MasterModuleInfos)
                {
                    await AddModuleAsync(masterModule, solutionFile);
                }
            }
        }

        private async Task<string> AddNewFolderAndGetIdOrGetExistingIdAsync(string solutionFile, string folderName,
            string parentFolderId = null)
        {
            var file = File.ReadAllText(solutionFile);
            var lines = file.Split(Environment.NewLine).ToList();
            string folderId;

            var folderLineIndex = lines.FindIndex(l =>
                l.Contains("2150E333-8FDC-42A3-9474-1A3956D46DE8") && l.Contains("\"" + folderName + "\""));

            if (folderLineIndex < 0)
            {
                folderId = Guid.NewGuid().ToString();
                var newFolderLine = "Project(\"{2150E333-8FDC-42A3-9474-1A3956D46DE8}\") = \"" + folderName + "\", \"" +
                                    folderName + "\", \"{" + folderId + "}\""
                                    + Environment.NewLine + "EndProject";

                lines.InsertAfter(l => l.Trim().Equals("EndProject"), newFolderLine);

                if (parentFolderId != null)
                {
                    var newPreSolutionLine =
                        "		{" + folderId + "} = {" + parentFolderId + "}";

                    lines.InsertAfter(l => l.Contains("GlobalSection") && l.Contains("NestedProjects"),
                        newPreSolutionLine);
                }
            }
            else
            {
                folderId = lines[folderLineIndex].Replace("\"", " ").Replace("{", " ").Replace("}", " ").TrimEnd()
                    .Split(" ").Last();
            }

            File.WriteAllText(solutionFile, string.Join(Environment.NewLine, lines));

            return folderId;
        }
    }
}
