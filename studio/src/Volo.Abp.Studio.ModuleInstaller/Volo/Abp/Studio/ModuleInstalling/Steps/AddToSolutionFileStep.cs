using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Studio.Modules;
using Volo.Abp.Studio.Solution;

namespace Volo.Abp.Studio.ModuleInstalling.Steps
{
    public class AddToSolutionFileStep : ModuleInstallingPipelineStep
    {
        public override async Task ExecuteAsync(ModuleInstallingContext context)
        {
            var slnFile = context.TargetModule.RemovePostFix(ModuleConsts.FileExtension) + ".sln";
            var moduleSrcFolder = Path.Combine(context.GetTargetSourceCodeFolder(), "src");
            var moduleTestFolder = Path.Combine(context.GetTargetSourceCodeFolder(), "test");

            await AddProjectsUnderDirectoryToSolutionFile(context, slnFile, moduleSrcFolder, $"modules/{context.ModuleName}");
            await AddProjectsUnderDirectoryToSolutionFile(context, slnFile, moduleTestFolder, $"test/{context.ModuleName}.Tests");
        }

        private static async Task AddProjectsUnderDirectoryToSolutionFile(
            ModuleInstallingContext context,
            string slnFile,
            string directory,
            string slnTargetFolder)
        {
            var _dotnetSlnFileModifier = context.ServiceProvider.GetRequiredService<IDotnetSlnFileModifierService>();

            var projects = Directory.GetFiles(directory, "*.csproj", SearchOption.AllDirectories);

            foreach (var project in projects)
            {
                await _dotnetSlnFileModifier.AddProjectAsync(slnFile, project, slnTargetFolder);
            }
        }
    }
}
