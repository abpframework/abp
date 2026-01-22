using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.ProjectModification;

public class SolutionFileModifier : ITransientDependency
{
    private readonly ICmdHelper _cmdHelper;

    public SolutionFileModifier(ICmdHelper cmdHelper)
    {
        _cmdHelper = cmdHelper;
    }

    public async Task RemoveProjectFromSolutionFileAsync(string solutionFile, string projectName)
    {
        var workingDirectory = Path.GetDirectoryName(solutionFile);
        var list = _cmdHelper.RunCmdAndGetOutput($"dotnet sln \"{solutionFile}\" list", workingDirectory: workingDirectory);

        foreach (var line in list.Split(new[] { Environment.NewLine, "\n" }, StringSplitOptions.None))
        {
            if (Path.GetFileNameWithoutExtension(line.Trim()).Equals(projectName, StringComparison.InvariantCultureIgnoreCase))
            {
                _cmdHelper.RunCmd($"dotnet sln \"{solutionFile}\" remove \"{line.Trim()}\"", workingDirectory: workingDirectory);
                break;
            }
        }
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
        _cmdHelper.RunCmd($"dotnet sln \"{solutionFile}\" add \"packages\\{package.Name}\\{package.Name}.csproj\" --solution-folder src");
    }

    private async Task AddModuleAsync(ModuleWithMastersInfo module, string solutionFile)
    {
        var slnDir = Path.GetDirectoryName(solutionFile);
        var projectsUnderModule = Directory.GetFiles(
            Path.Combine(slnDir, "modules", module.Name),
            "*.csproj",
            SearchOption.AllDirectories);

        var projectsUnderTest = new List<string>();
        if (Directory.Exists(Path.Combine(slnDir, "modules", module.Name, "test")))
        {
            projectsUnderTest = Directory.GetFiles(
                Path.Combine(slnDir, "modules", module.Name, "test"),
                "*.csproj",
                SearchOption.AllDirectories).ToList();
        }

        foreach (var projectPath in projectsUnderModule)
        {
            var folder = projectsUnderTest.Contains(projectPath) ? "test" : "src";
            var projectId = Path.GetFileName(projectPath).Replace(".csproj", "");
            var package = Path.Combine(slnDir, "modules", module.Name, folder, projectId, $"{projectId}.csproj");
            _cmdHelper.RunCmd($"dotnet sln \"{solutionFile}\" add \"{package}\" --solution-folder {folder}", workingDirectory: slnDir);
        }

        if (module.MasterModuleInfos != null)
        {
            foreach (var masterModule in module.MasterModuleInfos)
            {
                await AddModuleAsync(masterModule, solutionFile);
            }
        }
    }
}
