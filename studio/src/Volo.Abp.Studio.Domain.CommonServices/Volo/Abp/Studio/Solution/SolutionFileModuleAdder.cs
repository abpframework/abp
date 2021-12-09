using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Studio.Modules;

namespace Volo.Abp.Studio.Solution;

public class SolutionFileModuleAdder : ISolutionFileModuleAdder, ITransientDependency
{
    private readonly IDotnetSlnFileModifierService _dotnetSlnFileModifierService;

    public SolutionFileModuleAdder(IDotnetSlnFileModifierService dotnetSlnFileModifierService)
    {
        _dotnetSlnFileModifierService = dotnetSlnFileModifierService;
    }

    public async Task AddAsync(string TargetModule, string ModuleName)
    {
        var targetFolder = Path.Combine(Path.GetDirectoryName(TargetModule), "modules", ModuleName); ;
        var slnFile = TargetModule.RemovePostFix(ModuleConsts.FileExtension) + ".sln";
        var moduleSrcFolder = Path.Combine(targetFolder, "src");
        var moduleTestFolder = Path.Combine(targetFolder, "test");

        await AddProjectsUnderDirectoryToSolutionFile(slnFile, moduleSrcFolder, $"modules/{ModuleName}");
        await AddProjectsUnderDirectoryToSolutionFile(slnFile, moduleTestFolder, $"test/{ModuleName}.Tests");
    }

    private async Task AddProjectsUnderDirectoryToSolutionFile(
        string slnFile,
        string directory,
        string slnTargetFolder)
    {
        var projects = Directory.GetFiles(directory, "*.csproj", SearchOption.AllDirectories);

        foreach (var project in projects)
        {
            await _dotnetSlnFileModifierService.AddProjectAsync(slnFile, project, slnTargetFolder);
        }
    }
}
