using System;
using Microsoft.Extensions.Options;
using Volo.Abp.Storage.FileSystem.Internal;

namespace Volo.Abp.Storage.FileSystem.Server.Internal
{
    public class PublicUrlProvider : IPublicUrlProvider
    {
        private readonly AbpFileSystemStorageServerOptions _options;

        public PublicUrlProvider(IOptions<AbpFileSystemStorageServerOptions> options)
        {
            _options = options.Value;
        }

        public string GetPublicUrl(string storeName, FileSystemFileReference file)
        {
            var uriBuilder = new UriBuilder(_options.BaseUri);
            uriBuilder.Path = _options.EndpointPath.Add("/" + storeName).Add("/" + file.Path);

            return uriBuilder.ToString();
        }
    }
}