using Ionic.Zip;
using Volo.Abp.SolutionTemplating.Files;
using Volo.Abp.SolutionTemplating.Zipping;

namespace Volo.Abp.SolutionTemplating.Building.Steps
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
