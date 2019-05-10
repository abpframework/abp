using Ionic.Zip;
using Volo.Abp.Cli.ProjectBuilding.Files;
using Volo.Abp.Cli.ProjectBuilding.Zipping;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps
{
    public class CreateProjectResultZipStep : ProjectBuildPipelineStep
    {
        public override void Execute(ProjectBuildContext context)
        {
            context.Result.ZipContent = CreateZipFileFromEntries(context.Files);
        }

        private static byte[] CreateZipFileFromEntries(FileEntryList entries)
        {
            using (var resultZipFile = new ZipFile())
            {
                entries.CopyToZipFile(resultZipFile);
                return resultZipFile.GetBytes();
            }
        }
    }
}
