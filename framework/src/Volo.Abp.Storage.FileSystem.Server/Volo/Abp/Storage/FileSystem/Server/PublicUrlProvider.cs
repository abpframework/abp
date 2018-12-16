using System;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Storage.Configuration;

namespace Volo.Abp.Storage.FileSystem.Server
{
    public class PublicUrlProvider : IPublicUrlProvider, ITransientDependency
    {
        private readonly FileSystemStorageServerOptions _options;

        public PublicUrlProvider(IOptions<FileSystemStorageServerOptions> options)
        {
            _options = options.Value;
        }

        public string GetPublicUrl(string storeName, FileSystemFileReference file)
        {
            var uriBuilder = new UriBuilder(_options.BaseUri)
            {
                Path = _options.EndpointPath.Add("/" + storeName).Add("/" + file.Path)
            };

            return uriBuilder.ToString();
        }
    }
}
