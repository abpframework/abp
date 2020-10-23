using System;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerUI;
using Volo.Abp.VirtualFileSystem;

namespace Microsoft.AspNetCore.Builder
{
    public static class AbpSwaggerUIBuilderExtensions
    {
        public static IApplicationBuilder UseAbpSwaggerUI(
            this IApplicationBuilder app,
            Action<SwaggerUIOptions> setupAction = null)
        {
            var fileProvider = app.ApplicationServices.GetService<IVirtualFileProvider>();

            return app.UseSwaggerUI(options =>
            {
                options.IndexStream = () => fileProvider.GetFileInfo("/wwwroot/swagger/ui/index.html").CreateReadStream();

                setupAction?.Invoke(options);
            });
        }
    }
}
