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

        public IFileInfo GetFileInfo(string subpath)
        {
            return _fileProvider.GetFileInfo(subpath);
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            return _fileProvider.GetDirectoryContents(subpath);
        }

        public IChangeToken Watch(string filter)
        {
            return _fileProvider.Watch(filter);
        }

        private IFileProvider CreateHybridProvider()
        {
            IFileProvider fileProvider = new InternalVirtualFileProvider(_options);

            if (_options.FileSets.PhysicalPaths.Any())
            {
                var fileProviders = _options.FileSets.PhysicalPaths
                    .Select(p => new PhysicalFileProvider(p))
                    .Cast<IFileProvider>()
                    .ToList();

                fileProviders.Add(fileProvider);

                fileProvider = new CompositeFileProvider(fileProviders);
            }

            return fileProvider;
        }

        private class InternalVirtualFileProvider : IFileProvider
        {
            private readonly VirtualFileSystemOptions _options;
            private readonly Lazy<Dictionary<string, IFileInfo>> _files;

            public InternalVirtualFileProvider(VirtualFileSystemOptions options)
            {
                _options = options;
                _files = new Lazy<Dictionary<string, IFileInfo>>(
                    CreateResourcesDictionary,
                    true
                );
            }

            public IFileInfo GetFileInfo(string subpath)
            {
                if (string.IsNullOrEmpty(subpath))
                {
                    return new NotFoundFileInfo(subpath);
                }

                var file = _files.Value.GetOrDefault(VirtualFilePathHelper.NormalizePath(subpath));

                if (file == null)
                {
                    return new NotFoundFileInfo(subpath);
                }

                return file;
            }

            public IDirectoryContents GetDirectoryContents(string subpath)
            {
                var directory = GetFileInfo(subpath);
                if (!directory.IsDirectory)
                {
                    return new NotFoundDirectoryContents();
                }

                var fileList = new List<IFileInfo>();

                var directoryPath = subpath.EnsureEndsWith('/');
                foreach (var fileInfo in _files.Value.Values)
                {
                    if (!fileInfo.PhysicalPath.StartsWith(directoryPath))
                    {
                        continue;
                    }

                    var relativePath = fileInfo.PhysicalPath.Substring(directoryPath.Length);
                    if (relativePath.Contains("/"))
                    {
                        continue;
                    }

                    fileList.Add(fileInfo);
                }

                return new EnumerableDirectoryContents(fileList);
            }

            public IChangeToken Watch(string filter)
            {
                return NullChangeToken.Singleton;
            }

            private Dictionary<string, IFileInfo> CreateResourcesDictionary()
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