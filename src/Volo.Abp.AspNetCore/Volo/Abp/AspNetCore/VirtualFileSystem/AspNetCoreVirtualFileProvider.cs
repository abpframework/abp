using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using Volo.Abp.DependencyInjection;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.AspNetCore.VirtualFileSystem
{
    public class AspNetCoreVirtualFileProvider : IFileProvider
    {
        private readonly Lazy<IFileProvider> _fileProvider;
        private readonly IObjectAccessor<IServiceProvider> _serviceProviderAccessor;
        private readonly string _contentPath;

        public AspNetCoreVirtualFileProvider(IServiceProvider serviceProvider, string contentPath = null)
            : this(new ObjectAccessor<IServiceProvider>(serviceProvider))
        {
            _contentPath = contentPath;
        }

        public AspNetCoreVirtualFileProvider(IObjectAccessor<IServiceProvider> serviceProviderAccessor)
        {
            _serviceProviderAccessor = serviceProviderAccessor;
            _fileProvider = new Lazy<IFileProvider>(
                () => serviceProviderAccessor.Value.GetRequiredService<IVirtualFileProvider>(),
                true
            );
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            if (_contentPath != null)
            {
                subpath = _contentPath + subpath;
            }

            if (!IsInitialized())
            {
                return new NotFoundFileInfo(subpath);
            }

            return _fileProvider.Value.GetFileInfo(subpath);
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            if (_contentPath != null)
            {
                subpath = _contentPath + subpath;
            }

            if (!IsInitialized())
            {
                return new NotFoundDirectoryContents();
            }

            return _fileProvider.Value.GetDirectoryContents(subpath);
        }

        public IChangeToken Watch(string filter)
        {
            if (!IsInitialized())
            {
                return NullChangeToken.Singleton;
            }

            return _fileProvider.Value.Watch(filter);
        }

        private bool IsInitialized()
        {
            return _serviceProviderAccessor.Value != null;
        }
    }
}