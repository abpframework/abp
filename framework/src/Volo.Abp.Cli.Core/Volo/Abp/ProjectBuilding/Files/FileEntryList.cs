using System.Collections.Generic;

namespace Volo.Abp.ProjectBuilding.Files
{
    public class FileEntryList : List<FileEntry>
    {
        public FileEntryList(IEnumerable<FileEntry> entries)
            : base(entries)
        {
            
        }
    }
}