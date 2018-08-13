using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.Storage.Configuration;
using Volo.Abp.Storage.FileSystem.Configuration;

namespace Volo.Abp.Storage.FileSystem.Server
{
    public class AbpFileSystemStorageServerMiddleware
    {
        private readonly AbpFileSystemParsedOptions _fileSystemParsedOptions;

        private ILogger<AbpFileSystemStorageServerMiddleware> _logger;
        private RequestDelegate _next;
        private IOptions<AbpFileSystemStorageServerOptions> _serverOptions;

        public AbpFileSystemStorageServerMiddleware(RequestDelegate next,
            IOptions<AbpFileSystemStorageServerOptions> serverOptions,
            ILogger<AbpFileSystemStorageServerMiddleware> logger,
            IOptions<AbpFileSystemParsedOptions> fileSystemParsedOptions)
        {
            _fileSystemParsedOptions = fileSystemParsedOptions.Value;
            _next = next;
            _serverOptions = serverOptions;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var subPathStart = context.Request.Path.Value.IndexOf('/', 1);
            if (subPathStart > 0)
            {
                var storeName = context.Request.Path.Value.Substring(1, subPathStart - 1);
                var storageFactory = context.RequestServices.GetRequiredService<IAbpStorageFactory>();

                if (_fileSystemParsedOptions.ParsedStores.TryGetValue(storeName, out var storeOptions)
                    && storeOptions.ProviderType == AbpFileSystemStorageProvider.ProviderName)
                {
                    if (storeOptions.AccessLevel != BlobAccessLevel.Public)
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        return;
                    }

                    var store = storageFactory.GetStore(storeName, storeOptions);

                    var file = await store.GetBlobAsync(context.Request.Path.Value.Substring(subPathStart + 1), true);
                    if (file != null)
                    {
                        context.Response.ContentType = file.BlobDescriptor.ContentType;
                        context.Response.StatusCode = StatusCodes.Status200OK;

                        if (!string.IsNullOrEmpty(file.BlobDescriptor.ETag))
                            context.Response.Headers.Add("ETag", new[] {file.BlobDescriptor.ETag});

                        await file.ReadBlobToStreamAsync(context.Response.Body);
                        return;
                    }
                }
            }

            context.Response.StatusCode = StatusCodes.Status404NotFound;
        }
    }
}