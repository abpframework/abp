using System.IO;
using System.Reflection;
using Microsoft.Extensions.FileProviders;

namespace Volo.Abp.VirtualFileSystem
{
    public class VirtualFileSetInfo
    {
        public IFileProvider FileProvider
        {
            get => _fileProvider;
            set => _fileProvider = Check.NotNull(value, nameof(value));
        }
        private IFileProvider _fileProvider;

        public VirtualFileSetInfo(IFileProvider fileProvider)
        {
            FileProvider = fileProvider;
        }
    }

    public class EmbeddedVirtualFileSetInfo : VirtualFileSetInfo
    {
        public Assembly Assembly { get; }

        public string BaseFolder { get; }

        public EmbeddedVirtualFileSetInfo(IFileProvider fileProvider, Assembly assembly, string baseFolder = null) 
            : base(fileProvider)
        {
            Assembly = assembly;
            BaseFolder = baseFolder;
        }
    }
}