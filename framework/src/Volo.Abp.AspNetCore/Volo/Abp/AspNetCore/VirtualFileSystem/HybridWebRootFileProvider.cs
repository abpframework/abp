using System;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Volo.Abp.DependencyInjection;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.AspNetCore.VirtualFileSystem
{
    //TODO: How to handle wwwroot naming?
    public class HybridWebRootFileProvider : IHybridWebRootFileProvider, ISingletonDependency
    {
        private readonly IVirtualFileProvider _virtualFileProvider;
        private readonly IFileProvider _fileProvider;
        private readonly IHostingEnvironment _hostingEnvironment;
        private string _rootPath = "/wwwroot";

        protected AspNetCoreContentOptions Options { get; }

        public HybridWebRootFileProvider(
            IVirtualFileProvider virtualFileProvider, 
            IHostingEnvironment hostingEnvironment,
            IOptions<AspNetCoreContentOptions> options)
        {
            _virtualFileProvider = virtualFileProvider;
            _hostingEnvironment = hostingEnvironment;
            Options = options.Value;

            _fileProvider = CreateHybridProvider();
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            Check.NotNullOrEmpty(subpath, nameof(subpath));

            if (ExtraAllowedFolder(subpath) && ExtraAllowedExtension(subpath))
            {
                var fileInfo = _fileProvider.GetFileInfo(subpath);
                if (fileInfo.Exists)
                {
                    return fileInfo;
                }
            }

            return _fileProvider.GetFileInfo(_rootPath + subpath);
        }

        public IDirectoryContents GetDirectoryContents([NotNull] string subpath)
        {
            Check.NotNullOrEmpty(subpath, nameof(subpath));

            if (ExtraAllowedFolder(subpath))
            {
                var directory = _fileProvider.GetDirectoryContents(subpath);
                if (directory.Exists)
                {
                    return directory;
                }
            }

            return _fileProvider.GetDirectoryContents(_rootPath + subpath);
        }

        public virtual IChangeToken Watch(string filter)
        {
            if (!ExtraAllowedFolder(filter))
            {
                return _fileProvider.Watch("/wwwroot" + filter);
            }

            return new CompositeChangeToken(
                new[]
                {
                    _fileProvider.Watch("/wwwroot" + filter),
                    _fileProvider.Watch(filter)
                }
            );
        }

        protected virtual IFileProvider CreateHybridProvider()
        {
            return new CompositeFileProvider(
                new PhysicalFileProvider(_hostingEnvironment.ContentRootPath),
                _virtualFileProvider
            );
        }

        protected virtual bool ExtraAllowedFolder(string path)
        {
            return Options.AllowedExtraWebContentFolders.Any(s => path.StartsWith(s, StringComparison.OrdinalIgnoreCase));
        }

        protected virtual bool ExtraAllowedExtension(string path)
        {
            return Options.AllowedExtraWebContentFileExtensions.Any(e => path.EndsWith(e, StringComparison.OrdinalIgnoreCase));
        }
    }
}