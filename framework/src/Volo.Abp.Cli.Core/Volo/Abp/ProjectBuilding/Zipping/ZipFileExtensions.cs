using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ionic.Zip;
using Volo.Abp.ProjectBuilding.Files;

namespace Volo.Abp.ProjectBuilding.Zipping
{
    public static class ZipFileExtensions
    {
        public static byte[] GetBytes(this ZipFile zipFile)
        {
            using (var ms = new MemoryStream())
            {
                zipFile.Save(ms);
                return ms.ToArray();
            }
        }

        public static FileEntryList ToFileEntryList(this ZipFile zipFile, string rootFolder = null)
        {
            var zipEntries = zipFile.Entries;

            if (rootFolder != null)
            {
                zipEntries = zipEntries.Where(entry => entry.FileName.StartsWith(rootFolder)).ToList();
            }

            var fileEntries = new List<FileEntry>();

            foreach (var zipEntry in zipEntries)
            {
                using (var entryStream = zipEntry.OpenReader())
                {
                    var fileName = zipEntry.FileName;

                    if (rootFolder != null)
                    {
                        fileName = fileName.RemovePreFix(rootFolder);
                    }

                    if (fileName.IsNullOrEmpty())
                    {
                        continue;
                    }

                    fileName = fileName.EnsureStartsWith('/');
                    
                    fileEntries.Add(new FileEntry(fileName, entryStream.GetAllBytes(), zipEntry.IsDirectory));
                }
            }

            return new FileEntryList(fileEntries);
        }
    }
}
