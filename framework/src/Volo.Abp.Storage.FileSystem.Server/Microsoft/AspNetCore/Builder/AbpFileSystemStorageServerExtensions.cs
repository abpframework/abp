using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Storage.Configuration;
using Volo.Abp.Storage.FileSystem.Server;

namespace Microsoft.AspNetCore.Builder
{
    public static class AbpFileSystemStorageServerExtensions
    {
        public static IApplicationBuilder UseAbpFileSystemStorageServer(this IApplicationBuilder app)
        {
            var options = app.ApplicationServices.GetRequiredService<IOptions<FileSystemStorageServerOptions>>();

            app.Map(
                options.Value.EndpointPath,
                storePipeline =>
                {
                    storePipeline.UseMiddleware<FileSystemStorageServerMiddleware>();
                });

            return app;
        }
    }
}
