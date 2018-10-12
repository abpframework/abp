using System.Collections.Generic;

namespace Volo.Utils.SolutionTemplating.Files
{
    public class FileEntryList : List<FileEntry>
    {
        public FileEntryList(IEnumerable<FileEntry> entries)
            : base(entries)
        {
            
        }
    }
}