using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.VirtualFileSystem;

namespace Microsoft.AspNetCore.Builder
{
    public static class VirtualFileSystemApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseVirtualFiles(this IApplicationBuilder app, Action<StaticFileOptions> configure = null)
        {
            var staticFileOptions = new StaticFileOptions
            {
                FileProvider = app.ApplicationServices.GetRequiredService<IWebContentFileProvider>(),
                ContentTypeProvider = app.ApplicationServices.GetRequiredService<AbpFileExtensionContentTypeProvider>()
            };

            configure?.Invoke(staticFileOptions);

            return app.UseStaticFiles(staticFileOptions);
        }
    }
}
