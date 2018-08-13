using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Storage.FileSystem.Server.Internal;

namespace Volo.Abp.Storage.FileSystem.Server
{
    public static class AbpFileSystemStorageServerExtensions
    {
        public static IServiceCollection AddFileSystemStorageServer([NotNull] this IServiceCollection services,
            Action<AbpFileSystemStorageServerOptions> configure)
        {
            services.Configure(configure);
            services.AddTransient<IPublicUrlProvider, PublicUrlProvider>();
            return services;
        }

        public static IApplicationBuilder UseFileSystemStorageServer([NotNull] this IApplicationBuilder app)
        {
            var options = app.ApplicationServices.GetRequiredService<IOptions<AbpFileSystemStorageServerOptions>>();
            app.Map(options.Value.EndpointPath,
                storePipeline => { storePipeline.UseMiddleware<AbpFileSystemStorageServerMiddleware>(); });

            return app;
        }
    }
}