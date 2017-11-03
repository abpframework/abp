using System.Collections.Generic;
using Microsoft.Extensions.FileProviders;

namespace Volo.Abp.VirtualFileSystem
{
    public interface IVirtualFileSet
    {
        void AddFiles(Dictionary<string, IFileInfo> files);
    }
}