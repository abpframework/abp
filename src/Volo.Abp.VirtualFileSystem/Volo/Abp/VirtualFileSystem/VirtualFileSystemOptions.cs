using System.Collections.Generic;

namespace Volo.Abp.VirtualFileSystem
{
    public class VirtualFileSystemOptions
    {
        public List<IVirtualFileSet> FileSets { get; }

        public VirtualFileSystemOptions()
        {
            FileSets = new List<IVirtualFileSet>();
        }
    }
}