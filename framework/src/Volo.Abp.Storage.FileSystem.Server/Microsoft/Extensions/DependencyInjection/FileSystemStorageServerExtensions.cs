using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Volo.Abp.Storage.Configuration;
using Volo.Abp.Storage.FileSystem;
using Volo.Abp.Storage.FileSystem.Server;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class FileSystemStorageServerExtensions
    {
        public static IServiceCollection AddFileSystemStorageServer(this IServiceCollection services,
            Action<FileSystemStorageServerOptions> configure)
        {
            services.Configure<FileSystemStorageServerOptions>(configure);
            services.AddTransient<IPublicUrlProvider, PublicUrlProvider>();
            return services;
        }

        public static IApplicationBuilder UseFileSystemStorageServer(this IApplicationBuilder app)
        {
            var options = app.ApplicationServices.GetRequiredService<IOptions<FileSystemStorageServerOptions>>();
            app.Map(options.Value.EndpointPath,
                storePipeline => { storePipeline.UseMiddleware<FileSystemStorageServerMiddleware>(); });

            return app;
        }
    }
}