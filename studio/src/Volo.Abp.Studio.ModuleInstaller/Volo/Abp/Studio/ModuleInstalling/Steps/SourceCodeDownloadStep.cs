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

            var targetFolder = context.GetTargetSourceCodeFolder();

            CreateTargetDirectory(targetFolder);

            using (var archive = ZipFile.OpenRead(zipFilePath))
            {
                foreach (var entry in archive.Entries)
                {
                    entry.ExtractToFile(Path.Combine(targetFolder, entry.FullName));
                }
            }
        }

        private void CreateTargetDirectory(string targetFolder)
        {
            Directory.CreateDirectory(targetFolder);
        }
    }
}
