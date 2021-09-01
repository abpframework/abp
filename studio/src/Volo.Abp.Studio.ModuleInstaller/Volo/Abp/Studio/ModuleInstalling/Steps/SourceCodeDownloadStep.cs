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

            var sourceCodePackageName = $"{context.ModuleName}.SourceCode";

            var zipFilePath = await _nugetSourceCodeStoreManager.GetCachedSourceCodeFilePathAsync(
                sourceCodePackageName,
                SourceCodeTypes.Module,
                context.Version);

            var targetFolder = context.GetTargetSourceCodeFolder();

            using (var archive = ZipFile.OpenRead(zipFilePath))
            {
                foreach (var entry in archive.Entries)
                {
                    entry.ExtractToFile(Path.Combine(targetFolder, entry.FullName));
                }
            }
        }
    }
}
