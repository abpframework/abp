using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using Volo.Abp.Cli.ProjectBuilding.Files;

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
            using (var memoryStream = new MemoryStream())
            {
                using (var zipOutputStream = new ZipOutputStream(memoryStream))
                {
                    zipOutputStream.SetLevel(3); //0-9, 9 being the highest level of compression

                    foreach (var entry in entries)
                    {
                        zipOutputStream.PutNextEntry(new ZipEntry(entry.Name)
                        {
                            Size = entry.Bytes.Length
                        });
                        zipOutputStream.Write(entry.Bytes, 0, entry.Bytes.Length);
                    }

                    zipOutputStream.CloseEntry();
                    zipOutputStream.IsStreamOwner = false;
                }

                memoryStream.Position = 0;
                return memoryStream.ToArray();
            }
        }
    }
}
