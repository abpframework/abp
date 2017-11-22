using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Volo.Abp.DependencyInjection;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.AspNetCore.VirtualFileSystem
{
    public class AspNetCoreVirtualFileProvider : IFileProvider
    {
        private readonly Lazy<IVirtualFileProvider> _virtualFileProvider;
        private readonly Lazy<AspNetCoreVirtualFileOptions> _options;
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

            _virtualFileProvider = new Lazy<IVirtualFileProvider>(
                () => serviceProviderAccessor.Value.GetRequiredService<IVirtualFileProvider>(),
                true
            );

            _options = new Lazy<AspNetCoreVirtualFileOptions>(
                () => serviceProviderAccessor.Value.GetRequiredService<IOptions<AspNetCoreVirtualFileOptions>>().Value,
                true
            );
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            if (_contentPath != null)
            {
                subpath = _contentPath + subpath;
            }

            //TODO: Ignore files in _options.Value.IgnoredFileExtensions

            if (!IsInitialized())
            {
                return new NotFoundFileInfo(subpath);
            }

            return _virtualFileProvider.Value.GetFileInfo(subpath);
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            if (_contentPath != null)
            {
                subpath = _contentPath + subpath;
            }

            //TODO: Ignore files in _options.Value.IgnoredFileExtensions

            if (!IsInitialized())
            {
                return new NotFoundDirectoryContents();
            }

            return _virtualFileProvider.Value.GetDirectoryContents(subpath);
        }

        public IChangeToken Watch(string filter)
        {
            if (!IsInitialized())
            {
                return NullChangeToken.Singleton;
            }

            return _virtualFileProvider.Value.Watch(filter);
        }

        private bool IsInitialized()
        {
            return _serviceProviderAccessor.Value != null;
        }
    }
}