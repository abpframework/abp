using Ionic.Zip;
using Volo.Abp.ProjectBuilding.Files;
using Volo.Abp.ProjectBuilding.Zipping;

namespace Volo.Abp.ProjectBuilding.Building.Steps
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
