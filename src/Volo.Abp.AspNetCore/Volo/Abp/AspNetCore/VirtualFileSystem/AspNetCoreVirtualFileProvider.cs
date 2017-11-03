using System;
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
        private readonly Lazy<IVirtualFileProvider> _embeddedResourceManager;
        private readonly Lazy<AspNetCoreVirtualFileOptions> _options;
        private readonly IObjectAccessor<IServiceProvider> _serviceProviderAccessor;

        public AspNetCoreVirtualFileProvider(IServiceProvider serviceProvider)
            : this(new ObjectAccessor<IServiceProvider>(serviceProvider))
        {

        }

        public AspNetCoreVirtualFileProvider(IObjectAccessor<IServiceProvider> serviceProviderAccessor)
        {
            _serviceProviderAccessor = serviceProviderAccessor;

            _embeddedResourceManager = new Lazy<IVirtualFileProvider>(
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
            //TODO: Ignore files in _options.Value.IgnoredFileExtensions

            if (!IsInitialized())
            {
                return new NotFoundFileInfo(subpath);
            }

            return _embeddedResourceManager.Value.GetFileInfo(subpath);
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            //TODO: Ignore files in _options.Value.IgnoredFileExtensions

            if (!IsInitialized())
            {
                return new NotFoundDirectoryContents();
            }

            return _embeddedResourceManager.Value.GetDirectoryContents(subpath);
        }

        public IChangeToken Watch(string filter)
        {
            if (!IsInitialized())
            {
                return NullChangeToken.Singleton;
            }

            return _embeddedResourceManager.Value.Watch(filter);
        }

        private bool IsInitialized()
        {
            return _serviceProviderAccessor.Value != null;
        }
    }
}