using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Cli.ProjectBuilding;
using Volo.Abp.Studio.Nuget;

namespace Volo.Abp.Studio.ModuleInstalling.Steps
{
    public class SourceCodeDownloadStep : ModuleInstallingPipelineStep
    {
        public override async Task ExecuteAsync(ModuleInstallingContext context)
        {
            var _nugetSourceCodeStoreManager = context.ServiceProvider.GetRequiredService<INugetSourceCodeStore>();

            var zipFilePath = await _nugetSourceCodeStoreManager.GetCachedSourceCodeFilePathAsync(
                context.ModuleName,
                SourceCodeTypes.Module,
                context.Version);

            if (zipFilePath == null)
            {
                throw new AbpStudioException(message: $"Source code not found for {context.ModuleName} (v{context.Version})");
            }

            var targetFolder = context.GetTargetSourceCodeFolder();

            if (Directory.Exists(targetFolder))
            {
                return;
            }

            Directory.CreateDirectory(targetFolder);

            ZipFile.ExtractToDirectory(zipFilePath, targetFolder);
        }
    }
}
