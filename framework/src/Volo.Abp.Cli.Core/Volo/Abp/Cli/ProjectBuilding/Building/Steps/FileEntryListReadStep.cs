using System;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Volo.Abp.Cli.ProjectBuilding.Files;

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
            var fileEntries = new List<FileEntry>();

            using (var memoryStream = new MemoryStream(fileBytes))
            {
                using (var zipInputStream = new ZipInputStream(memoryStream))
                {
                    var zipEntry = zipInputStream.GetNextEntry();
                    while (zipEntry != null)
                    {
                        var buffer = new byte[4096]; // 4K is optimum

                        using (var fileEntryMemoryStream = new MemoryStream())
                        {
                            StreamUtils.Copy(zipInputStream, fileEntryMemoryStream, buffer);
                            fileEntries.Add(new FileEntry(zipEntry.Name.EnsureStartsWith('/'), fileEntryMemoryStream.ToArray(), zipEntry.IsDirectory));
                        }

                        zipEntry = zipInputStream.GetNextEntry();
                    }
                }

                return new FileEntryList(fileEntries);
            }
        }
    }
}