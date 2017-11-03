using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.VirtualFileSystem
{
    internal class AspNetCoreVirtualFileOptions //TODO: and use this!
    {
        public HashSet<string> IgnoredFileExtensions { get; }

        public AspNetCoreVirtualFileOptions()
        {
            IgnoredFileExtensions = new HashSet<string>
            {
                ".cshtml",
                ".config"
            };
        }
    }
}