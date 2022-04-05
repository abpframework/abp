using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Volo.Abp.Cli.ProjectBuilding.Files;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps;

public class FileEntryListReadStep : ProjectBuildPipelineStep
{
    public override void Execute(ProjectBuildContext context)
    {
        context.Files = GetEntriesFromZipFile(context.TemplateFile.FileBytes);
    }

    private static FileEntryList GetEntriesFromZipFile(byte[] fileBytes)
    {
        var fileEntries = new List<FileEntry>();

        using (var memoryStream = new MemoryStream(fileBytes))
        {
            using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Read))
            {
                foreach (var zipEntry in zipArchive.Entries)
                {
                    using (var entryStream = zipEntry.Open())
                    using (var fileEntryMemoryStream = new MemoryStream())
                    {
                        entryStream.CopyTo(fileEntryMemoryStream);
                        var isDirectory =  zipEntry.FullName.EndsWith("/") || zipEntry.FullName.EndsWith("\\");
                        fileEntries.Add(new FileEntry(zipEntry.FullName.EnsureStartsWith('/'), fileEntryMemoryStream.ToArray(), isDirectory));
                    }
                }
            }

            return new FileEntryList(fileEntries);
        }
    }
}
