using Volo.Abp.AspNetCore.MultiTenancy;

namespace Microsoft.AspNetCore.Builder
{
    public static class AbpAspNetCoreMultiTenancyApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseMultiTenancy(this IApplicationBuilder app)
        {
            return app
                .UseMiddleware<MultiTenancyMiddleware>();
        }
    }
}
