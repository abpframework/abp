using System.IO;
using Ionic.Zip;
using Volo.Abp.Cli.ProjectBuilding.Files;
using Volo.Abp.Cli.ProjectBuilding.Zipping;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps
{
    public class FileEntryListReadStep : ProjectBuildPipelineStep
    {
        public override void Execute(ProjectBuildContext context)
        {
            context.Files = GetEntriesFromZipFile(context.TemplateFile.FileBytes);
        }

        private static FileEntryList GetEntriesFromZipFile(byte[] fileBytes)
        {
            using (var memoryStream = new MemoryStream(fileBytes))
            {
                using (var templateZipFile = ZipFile.Read(memoryStream))
                {
                    return templateZipFile.ToFileEntryList();
                }
            }
        }
    }
}