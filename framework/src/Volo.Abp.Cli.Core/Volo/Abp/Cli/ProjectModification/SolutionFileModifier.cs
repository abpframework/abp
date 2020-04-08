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
        public async Task AddModuleToSolutionFileAsync(ModuleWithMastersInfo module, string solutionFile)
        {
            await AddModule(module, solutionFile);
        }

        private async Task AddModule(ModuleWithMastersInfo module, string solutionFile)
        {
            var srcModuleFolderId = await AddNewFolderAndGetIdOrGetExistingId(solutionFile, module.Name, await AddNewFolderAndGetIdOrGetExistingId(solutionFile, "modules"));
            var testModuleFolderId = await AddNewFolderAndGetIdOrGetExistingId(solutionFile, module.Name + ".Tests", await AddNewFolderAndGetIdOrGetExistingId(solutionFile, "test"));
            
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
                var projectId = Path.GetFileName(projectPath).Replace(".csproj","");
                var projectParentFolderInModule = projectsUnderTest.Contains(projectPath) ? "test" : "src";

                if (lines.Any(l => l.Contains($"\"{projectId}\"")))
                {
                    continue;
                }

                var projectGuid = Guid.NewGuid().ToString();

                var newProjectLine = "Project(\"{9A19103F-16F7-4668-BE54-9A1E7A4F7556}\") = \"" + projectId + "\"," +
                              " \"modules\\" + module.Name + "\\"+ projectParentFolderInModule + "\\" + projectId + "\\" + projectId + ".csproj\", \"{" + projectGuid + "}\""
                              + Environment.NewLine + "EndProject";

                lines.InsertAfter(l => l.Trim().Equals("EndProject"), newProjectLine);
                
                var newPostSolutionLine =
                    "		{" + projectGuid + "}.Debug|Any CPU.ActiveCfg = Debug|Any CPU" + Environment.NewLine +
                    "		{" + projectGuid + "}.Debug|Any CPU.Build.0 = Debug|Any CPU" + Environment.NewLine +
                    "		{" + projectGuid + "}.Release|Any CPU.ActiveCfg = Release|Any CPU" + Environment.NewLine +
                    "		{" + projectGuid + "}.Release|Any CPU.Build.0 = Release|Any CPU";

                lines.InsertAfter(l=>l.Contains("GlobalSection") && l.Contains("ProjectConfigurationPlatforms"), newPostSolutionLine);
                
                var newPreSolutionLine =
                    "		{"+ projectGuid + "} = {"+ parentFolderId + "}";

                lines.InsertAfter(l=>l.Contains("GlobalSection") && l.Contains("NestedProjects"), newPreSolutionLine);
            }

            File.WriteAllText(solutionFile, string.Join(Environment.NewLine, lines));

            if (module.MasterModuleInfos != null)
            {
                foreach (var masterModule in module.MasterModuleInfos)
                {
                    await AddModule(masterModule, solutionFile);
                }
            }
        }

        private async Task<string> AddNewFolderAndGetIdOrGetExistingId(string solutionFile, string folderName, string parentFolderId = null)
        {
            var file = File.ReadAllText(solutionFile);
            var lines = file.Split(Environment.NewLine).ToList();
            string folderId;

            var folderLineIndex = lines.FindIndex(l =>
                l.Contains("2150E333-8FDC-42A3-9474-1A3956D46DE8") && l.Contains("\""+ folderName + "\""));

            if (folderLineIndex < 0)
            {
                folderId = Guid.NewGuid().ToString();
                var newFolderLine = "Project(\"{2150E333-8FDC-42A3-9474-1A3956D46DE8}\") = \""+ folderName + "\", \""+ folderName + "\", \"{" + folderId + "}\""
                                    + Environment.NewLine + "EndProject";

                lines.InsertAfter(l => l.Trim().Equals("EndProject"), newFolderLine);

                if (parentFolderId != null)
                {
                    var newPreSolutionLine =
                        "		{" + folderId + "} = {" + parentFolderId + "}";

                    lines.InsertAfter(l => l.Contains("GlobalSection") && l.Contains("NestedProjects"), newPreSolutionLine);
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
