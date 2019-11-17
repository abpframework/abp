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
        private readonly IFileProvider _hybridFileProvider;
        private readonly AbpVirtualFileSystemOptions _options;

        public VirtualFileProvider(
            IOptions<AbpVirtualFileSystemOptions> options,
            IDynamicFileProvider dynamicFileProvider)
        {
            _options = options.Value;
            _hybridFileProvider = CreateHybridProvider(dynamicFileProvider);
        }

        public virtual IFileInfo GetFileInfo(string subpath)
        {
            return _hybridFileProvider.GetFileInfo(subpath);
        }

        public virtual IDirectoryContents GetDirectoryContents(string subpath)
        {
            return _hybridFileProvider.GetDirectoryContents(subpath);
        }

        public virtual IChangeToken Watch(string filter)
        {
            return _hybridFileProvider.Watch(filter);
        }

        protected virtual IFileProvider CreateHybridProvider(IDynamicFileProvider dynamicFileProvider)
        {
            var fileProviders = new List<IFileProvider>();

            fileProviders.Add(dynamicFileProvider);

            if (_options.FileSets.PhysicalPaths.Any())
            {
                fileProviders.AddRange(
                    _options.FileSets.PhysicalPaths
                        .Select(rootPath => new PhysicalFileProvider(rootPath))
                        .Reverse()
                );
            }

            fileProviders.Add(new InternalVirtualFileProvider(_options));

            return new CompositeFileProvider(fileProviders);
        }

        protected class InternalVirtualFileProvider : DictionaryBasedFileProvider
        {
            protected override IDictionary<string, IFileInfo> Files => _files.Value;

            private readonly AbpVirtualFileSystemOptions _options;
            private readonly Lazy<Dictionary<string, IFileInfo>> _files;

            public InternalVirtualFileProvider(AbpVirtualFileSystemOptions options)
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

            protected override string NormalizePath(string subpath)
            {
                return VirtualFilePathHelper.NormalizePath(subpath);
            }
        }
    }
}