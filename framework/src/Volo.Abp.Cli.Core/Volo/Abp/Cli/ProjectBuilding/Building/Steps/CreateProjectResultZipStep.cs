using System.IO;
using System.IO.Compression;
using Volo.Abp.Cli.ProjectBuilding.Files;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps;

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
            using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                foreach (var entry in entries)
                {
                    var zipEntry = zipArchive.CreateEntry(entry.Name, CompressionLevel.Fastest);
                    using (var stream = zipEntry.Open())
                    {
                        stream.Write(entry.Bytes, 0, entry.Bytes.Length);
                    }
                }
            }

            memoryStream.Position = 0;
            return memoryStream.ToArray();
        }
    }
}
