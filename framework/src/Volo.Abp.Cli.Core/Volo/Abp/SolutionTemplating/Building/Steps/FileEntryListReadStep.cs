using System.IO;
using Ionic.Zip;
using Volo.Abp.SolutionTemplating.Files;
using Volo.Abp.SolutionTemplating.Zipping;

namespace Volo.Abp.SolutionTemplating.Building.Steps
{
    public class FileEntryListReadStep : ProjectBuildPipelineStep
    {
        public override void Execute(ProjectBuildContext context)
        {
            context.Files = GetEntriesFromZipFile(context.Template.FilePath);
        }

        private static FileEntryList GetEntriesFromZipFile(string filePath, string rootFolder = null)
        {
            using (var templateFileStream = File.OpenRead(filePath))
            {
                using (var templateZipFile = ZipFile.Read(templateFileStream))
                {
                    return templateZipFile.ToFileEntryList(rootFolder);
                }
            }
        }
    }
}