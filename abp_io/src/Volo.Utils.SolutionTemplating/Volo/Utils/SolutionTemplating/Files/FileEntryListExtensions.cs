using System;
using Ionic.Zip;

namespace Volo.Utils.SolutionTemplating.Files
{
    public static class FileEntryListExtensions
    {
        public static void CopyToZipFile(this FileEntryList fileEntryList, ZipFile zipFile)
        {
            foreach (var entry in fileEntryList)
            {
                try
                {
                    zipFile.AddEntry(entry.Name, entry.Bytes);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }
}