using Volo.Abp.AspNetCore.Serilog;

namespace Microsoft.AspNetCore.Builder
{
    public static class AbpAspNetCoreSerilogApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseAbpSerilogEnrichers(this IApplicationBuilder app)
        {
            return app
                .UseMiddleware<AbpSerilogMiddleware>();
        }
    }
}