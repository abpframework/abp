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
            if (context.Template.RootPathInZipFile == null)
            {
                context.Files = GetEntriesFromZipFile(context.Template.FilePath);
                return;
            }

            var entryListCachePath = CreateCachePath(context);

            if (File.Exists(entryListCachePath))
            {
                context.Files = GetEntriesFromZipFile(entryListCachePath);
                return;
            }

            context.Files = GetEntriesFromZipFile(
                context.Template.FilePath,
                context.Template.RootPathInZipFile);

            SaveCachedEntries(entryListCachePath, context.Files);
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

        private static void SaveCachedEntries(string filePath, FileEntryList entries)
        {
            using (var resultZipFile = new ZipFile())
            {
                entries.CopyToZipFile(resultZipFile);
                resultZipFile.Save(filePath);
            }
        }

        private string CreateCachePath(ProjectBuildContext context)
        {
            return context.Template.FilePath.Replace(".zip", "-filtered.zip");
        }
    }
}