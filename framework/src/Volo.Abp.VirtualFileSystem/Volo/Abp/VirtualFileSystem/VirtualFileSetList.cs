using System.Collections.Generic;

namespace Volo.Abp.VirtualFileSystem
{
    public class VirtualFileSetList : List<IVirtualFileSet>
    {
        public List<string> PhysicalPaths { get; }

        public VirtualFileSetList()
        {
            PhysicalPaths = new List<string>();
        }
    }
}