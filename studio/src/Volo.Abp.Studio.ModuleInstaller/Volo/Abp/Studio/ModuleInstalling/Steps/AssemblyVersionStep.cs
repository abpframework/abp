using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Studio.Packages.Modifying;

namespace Volo.Abp.Studio.ModuleInstalling.Steps;

public class AssemblyVersionStep : ModuleInstallingPipelineStep
{
    public async override Task ExecuteAsync(ModuleInstallingContext context)
    {
        var moduleFolder = Path.GetDirectoryName(context.TargetModule);
        var commonPropsFilePath = Path.Combine(moduleFolder, "common.props");

        if (!File.Exists(commonPropsFilePath))
        {
            return;
        }

        var _csprojFileManager = context.ServiceProvider.GetRequiredService<ICsprojFileManager>();

        var csProjFiles = Directory.GetFiles(context.GetTargetSourceCodeFolder(), "*.csproj", SearchOption.AllDirectories);

        foreach (var csProjFile in csProjFiles)
        {
            await _csprojFileManager.AddAssemblyVersionAsync(csProjFile, context.Version);
            await _csprojFileManager.AddCopyLocalLockFileAssembliesAsync(csProjFile);
        }
    }
}
