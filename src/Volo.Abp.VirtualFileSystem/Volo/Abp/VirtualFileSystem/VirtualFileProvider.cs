using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.VirtualFileSystem
{
    public class VirtualFileProvider : IVirtualFileProvider, ISingletonDependency
    {
        private readonly IFileProvider _fileProvider;
        private readonly VirtualFileSystemOptions _options;

        public VirtualFileProvider(IOptions<VirtualFileSystemOptions> options)
        {
            _options = options.Value;
            _fileProvider = CreateHybridProvider();
        }

        public virtual IFileInfo GetFileInfo(string subpath)
        {
            return _fileProvider.GetFileInfo(subpath);
        }

        public virtual IDirectoryContents GetDirectoryContents(string subpath)
        {
            return _fileProvider.GetDirectoryContents(subpath);
        }

        public virtual IChangeToken Watch(string filter)
        {
            return _fileProvider.Watch(filter);
        }

        protected virtual IFileProvider CreateHybridProvider()
        {
            IFileProvider fileProvider = new InternalVirtualFileProvider(_options);

            if (_options.FileSets.PhysicalPaths.Any())
            {
                var fileProviders = _options.FileSets.PhysicalPaths
                    .Select(rootPath => new PhysicalFileProvider(rootPath))
                    .Reverse()
                    .Cast<IFileProvider>()
                    .ToList();

                fileProviders.Add(fileProvider);

                fileProvider = new CompositeFileProvider(fileProviders);
            }

            return fileProvider;
        }

        protected class InternalVirtualFileProvider : DictionaryBasedFileProvider
        {
            protected override Dictionary<string, IFileInfo> Files => _files.Value;

            private readonly VirtualFileSystemOptions _options;
            private readonly Lazy<Dictionary<string, IFileInfo>> _files;

            public InternalVirtualFileProvider(VirtualFileSystemOptions options)
            {
                _options = options;
                _files = new Lazy<Dictionary<string, IFileInfo>>(
                    CreateFiles,
                    true
                );
            }
            
            private Dictionary<string, IFileInfo> CreateFiles()
            {
                var files = new Dictionary<string, IFileInfo>(StringComparer.OrdinalIgnoreCase);

                foreach (var set in _options.FileSets)
                {
                    set.AddFiles(files);
                }

                return files;
            }
        }
    }
}