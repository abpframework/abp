using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Studio.Helpers;

namespace Volo.Abp.Studio.Solution
{
    public class DotnetSlnFileModifierService : IDotnetSlnFileModifierService
    {
        public async Task AddProjectAsync(string slnFilePath, string projectPath, string slnTargetFolder = "src")
        {
            var projectName = Path.GetFileName(projectPath).RemovePostFix(".csproj");
            var folderId = await GetOrAddFolderIdAsync(slnFilePath, slnTargetFolder);
            var slnFileLines = (await File.ReadAllTextAsync(slnFilePath))
                .Split(Environment.NewLine).ToList();

            if (slnFileLines.Any(l => l.Contains($"\"{projectName}\"")))
            {
                throw new AbpStudioException(AbpStudioErrorCodes.ProjectWithSameNameAlreadyExistInTheSolutionFile)
                    .WithData("Solution Path", slnFilePath)
                    .WithData("Project Name", projectName);
            }

            var projectId = Guid.NewGuid().ToString();
            var relativeProjectPath = PathHelper.GetRelativePath(slnFilePath, projectPath).Replace("/", "\\");
            var newProjectLine = "Project(\"{9A19103F-16F7-4668-BE54-9A1E7A4F7556}\") = \"" + projectName + "\"," +
                                 " \"" + relativeProjectPath + "\", \"{" + projectId + "}\""
                                 + Environment.NewLine + "EndProject";

            slnFileLines.InsertAfter(l => l.Trim().Equals("EndProject"), newProjectLine);

            var newPostSolutionLine =
                "		{" + projectId + "}.Debug|Any CPU.ActiveCfg = Debug|Any CPU" + Environment.NewLine +
                "		{" + projectId + "}.Debug|Any CPU.Build.0 = Debug|Any CPU" + Environment.NewLine +
                "		{" + projectId + "}.Release|Any CPU.ActiveCfg = Release|Any CPU" + Environment.NewLine +
                "		{" + projectId + "}.Release|Any CPU.Build.0 = Release|Any CPU";

            if (!slnFileLines.Any(l => l.Contains("GlobalSection") && l.Contains("ProjectConfigurationPlatforms")))
            {
                slnFileLines.InsertBefore(l => l.Trim().Equals("EndGlobal"), "	GlobalSection(ProjectConfigurationPlatforms) = postSolution");
                slnFileLines.InsertBefore(l => l.Trim().Equals("EndGlobal"), "	EndGlobalSection");
            }

            slnFileLines.InsertAfter(l => l.Contains("GlobalSection") && l.Contains("ProjectConfigurationPlatforms"),
                newPostSolutionLine);

            var newPreSolutionLine =
                "		{" + projectId + "} = {" + folderId + "}";


            if (!slnFileLines.Any(l => l.Contains("GlobalSection") && l.Contains("NestedProjects")))
            {
                slnFileLines.InsertBefore(l => l.Trim().Equals("EndGlobal"), "	GlobalSection(NestedProjects) = preSolution");
                slnFileLines.InsertBefore(l => l.Trim().Equals("EndGlobal"), "	EndGlobalSection");
            }

            slnFileLines.InsertAfter(l => l.Contains("GlobalSection") && l.Contains("NestedProjects"), newPreSolutionLine);

            await File.WriteAllTextAsync(slnFilePath, string.Join(Environment.NewLine, slnFileLines));
        }

        private async Task<string> GetOrAddFolderIdAsync(string solutionFile, string folderName, string parentFolderId = null)
        {
            if (folderName.Contains("/") && parentFolderId == null)
            {
                var parents = folderName.Split("/").SkipLast(1).JoinAsString("/");

                parentFolderId = await GetOrAddFolderIdAsync(solutionFile, parents);
            }

            var file = await File.ReadAllTextAsync(solutionFile);
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

                if (lines.Any(l => l.Trim().Equals("EndProject")))
                {
                    lines.InsertAfter(l => l.Trim().Equals("EndProject"), newFolderLine);
                }
                else
                {
                    lines.InsertAfter(l => l.StartsWith("Microsoft Visual Studio Solution File"), newFolderLine);
                }

                if (parentFolderId != null && lines.Any(l=>l.Contains("GlobalSection") && l.Contains("NestedProjects")))
                {
                    var newPreSolutionLine =
                        "		{" + folderId + "} = {" + parentFolderId + "}";

                    lines.InsertAfter(l => l.Contains("GlobalSection") && l.Contains("NestedProjects"),
                        newPreSolutionLine);
                }

                await File.WriteAllTextAsync(solutionFile, string.Join(Environment.NewLine, lines));
            }
            else
            {
                folderId = lines[folderLineIndex].Replace("\"", " ").Replace("{", " ").Replace("}", " ").TrimEnd()
                    .Split(" ").Last();
            }

            return folderId;
        }
    }
}
