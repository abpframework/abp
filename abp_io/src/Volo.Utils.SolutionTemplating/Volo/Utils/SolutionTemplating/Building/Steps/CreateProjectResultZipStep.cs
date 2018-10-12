using Ionic.Zip;
using Volo.Utils.SolutionTemplating.Files;
using Volo.Utils.SolutionTemplating.Zipping;

namespace Volo.Utils.SolutionTemplating.Building.Steps
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
