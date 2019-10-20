using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AspNetCore.MultiTenancy
{
    public static class TenantResolveContextExtensions
    {
        public static AbpAspNetCoreMultiTenancyOptions GetAbpAspNetCoreMultiTenancyOptions(this ITenantResolveContext context)
        {
            return context.ServiceProvider.GetRequiredService<IOptionsSnapshot<AbpAspNetCoreMultiTenancyOptions>>().Value;
        }
    }
}