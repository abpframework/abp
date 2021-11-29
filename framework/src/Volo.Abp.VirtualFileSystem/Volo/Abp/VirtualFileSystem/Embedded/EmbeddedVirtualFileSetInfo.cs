using System.Reflection;
using Microsoft.Extensions.FileProviders;

namespace Volo.Abp.VirtualFileSystem.Embedded
{
    public class EmbeddedVirtualFileSetInfo : VirtualFileSetInfo
    {
        public Assembly Assembly { get; }

        public string BaseFolder { get; }

        public EmbeddedVirtualFileSetInfo(
            IFileProvider fileProvider, 
            Assembly assembly,
            string baseFolder = null) 
            : base(fileProvider)
        {
            Assembly = assembly;
            BaseFolder = baseFolder;
        }
    }
}