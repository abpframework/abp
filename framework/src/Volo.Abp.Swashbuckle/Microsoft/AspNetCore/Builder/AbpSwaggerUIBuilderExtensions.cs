using System;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerUI;
using Volo.Abp.Swashbuckle;

namespace Microsoft.AspNetCore.Builder
{
    public static class AbpSwaggerUIBuilderExtensions
    {
        public static IApplicationBuilder UseAbpSwaggerUI(
            this IApplicationBuilder app,
            Action<SwaggerUIOptions> setupAction = null)
        {
            var resolver = app.ApplicationServices.GetService<ISwaggerHtmlResolver>();

            return app.UseSwaggerUI(options =>
            {
                options.InjectJavascript("/swagger/ui/abp.js");
                options.InjectJavascript("/swagger/ui/abp.swagger.js");
                options.IndexStream = () => resolver.Resolver();

                setupAction?.Invoke(options);
            });
        }
    }
}
