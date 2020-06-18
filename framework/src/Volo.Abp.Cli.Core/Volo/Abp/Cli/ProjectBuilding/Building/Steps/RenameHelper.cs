using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Cli.ProjectBuilding.Files;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps
{
    public static class RenameHelper
    {
        public static void RenameAll(List<FileEntry> entries, string placeHolder, string name)
        {
            RenameDirectoryRecursively(entries, placeHolder, name);
            RenameAllFiles(entries, placeHolder, name);
            ReplaceContent(entries, placeHolder, name);
        }

        private static void RenameDirectoryRecursively(List<FileEntry> entries, string placeHolder, string name)
        {
            foreach (var entry in entries.Where(e => e.IsDirectory))
            {
                if (entry.Name.Contains(placeHolder))
                {
                    entry.SetName(entry.Name.Replace(placeHolder, name));
                }
            }
        }

        private static void RenameAllFiles(List<FileEntry> entries, string placeHolder, string name)
        {
            foreach (var entry in entries.Where(e => !e.IsDirectory))
            {
                if (entry.Name.Contains(placeHolder))
                {
                    entry.SetName(entry.Name.Replace(placeHolder, name));
                }
            }
        }

        private static void ReplaceContent(List<FileEntry> entries, string placeHolder, string name)
        {
            foreach (var entry in entries.Where(e => !e.IsDirectory))
            {
                if (entry.Content.Length < placeHolder.Length)
                {
                    continue;
                }

                if (entry.IsBinaryFile)
                {
                    continue;
                }

                var newContent = entry.Content.Replace(placeHolder, name);
                if (newContent != entry.Content)
                {
                    entry.SetContent(newContent);
                }
            }
        }
    }
}
