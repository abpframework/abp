using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using Volo.Abp.DependencyInjection;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.AspNetCore.VirtualFileSystem
{
    public class HybridWebRootFileProvider : IHybridWebRootFileProvider, ISingletonDependency
    {
        private readonly IVirtualFileProvider _virtualFileProvider;
        private readonly IFileProvider _fileProvider;
        private readonly IHostingEnvironment _hostingEnvironment;

        public HybridWebRootFileProvider(IVirtualFileProvider virtualFileProvider, IHostingEnvironment hostingEnvironment)
        {
            _virtualFileProvider = virtualFileProvider;
            _hostingEnvironment = hostingEnvironment;

            _fileProvider = CreateHybridProvider();
        }

        public virtual IFileInfo GetFileInfo(string subpath)
        {
            return _fileProvider.GetFileInfo("/wwwroot" + subpath); //TODO: Hard-coded "/wwwroot" is not good!
        }

        public virtual IDirectoryContents GetDirectoryContents(string subpath)
        {
            return _fileProvider.GetDirectoryContents("/wwwroot" + subpath);
        }

        public virtual IChangeToken Watch(string filter)
        {
            return _fileProvider.Watch("/wwwroot" + filter);
        }

        public string GetAbsolutePath(string relativePath)
        {
            return Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot", relativePath.RemovePreFix("/"));
        }

        protected virtual IFileProvider CreateHybridProvider()
        {
            return new CompositeFileProvider(
                _hostingEnvironment.ContentRootFileProvider,
                _virtualFileProvider
            );
        }
    }
}