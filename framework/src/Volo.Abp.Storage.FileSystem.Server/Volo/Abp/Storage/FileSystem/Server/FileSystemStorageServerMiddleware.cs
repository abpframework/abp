using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.Storage.Configuration;

namespace Volo.Abp.Storage.FileSystem.Server
{
    // TODO: Refactore this!
    public class FileSystemStorageServerMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<FileSystemStorageServerMiddleware> _logger;
        private readonly IOptions<FileSystemStorageServerOptions> _serverOptions;
        private readonly FileSystemParsedOptions _fileSystemParsedOptions;

        public FileSystemStorageServerMiddleware(
            RequestDelegate next,
            IOptions<FileSystemStorageServerOptions> serverOptions,
            ILogger<FileSystemStorageServerMiddleware> logger,
            IOptions<FileSystemParsedOptions> fileSystemParsedOptions
            )
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
                    if (storeOptions.AccessLevel != AbpStorageAccessLevel.Public)
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        return;
                    }

                    var store = storageFactory.GetStore(storeName, storeOptions);

                    var file = await store.GetAsync(context.Request.Path.Value.Substring(subPathStart + 1), withMetadata: true);
                    if (file != null)
                    {
                        context.Response.ContentType = file.Properties.ContentType;
                        context.Response.StatusCode = StatusCodes.Status200OK;

                        if (!string.IsNullOrEmpty(file.Properties.ETag))
                        {
                            context.Response.Headers.Add("ETag", new[] { file.Properties.ETag });
                        }

                        await file.ReadToStreamAsync(context.Response.Body);
                        return;
                    }
                }
            }

            context.Response.StatusCode = StatusCodes.Status404NotFound;
        }
    }
}
