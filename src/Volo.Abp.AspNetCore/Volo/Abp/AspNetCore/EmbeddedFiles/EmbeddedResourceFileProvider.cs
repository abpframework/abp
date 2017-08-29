using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EmbeddedFiles;

namespace Volo.Abp.AspNetCore.EmbeddedFiles
{
    public class EmbeddedResourceFileProvider : IFileProvider
    {
        private readonly Lazy<IEmbeddedFileManager> _embeddedResourceManager;
        private readonly Lazy<AspNetCoreEmbeddedFileOptions> _options;

        public EmbeddedResourceFileProvider(IServiceProvider serviceProvider)
            : this(new ObjectAccessor<IServiceProvider>(serviceProvider))
        {
            
        }

        public EmbeddedResourceFileProvider(IObjectAccessor<IServiceProvider> serviceProvider)
        {
            _embeddedResourceManager = new Lazy<IEmbeddedFileManager>(
                () => serviceProvider.Value.GetRequiredService<IEmbeddedFileManager>(),
                true
            );

            _options = new Lazy<AspNetCoreEmbeddedFileOptions>(
                () => serviceProvider.Value.GetRequiredService<IOptions<AspNetCoreEmbeddedFileOptions>>().Value,
                true
            );
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            var resource = _embeddedResourceManager.Value.FindFile(subpath);

            if (resource == null || IsIgnoredFile(resource))
            {
                return new NotFoundFileInfo(subpath);
            }

            return new EmbeddedResourceItemFileInfo(resource);
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            //TODO: Implement...?

            return new NotFoundDirectoryContents();
        }

        public IChangeToken Watch(string filter)
        {
            return NullChangeToken.Singleton;
        }

        protected virtual bool IsIgnoredFile(EmbeddedFileInfo resource)
        {
            return resource.FileExtension != null && _options.Value.IgnoredFileExtensions.Contains(resource.FileExtension);
        }
    }
}