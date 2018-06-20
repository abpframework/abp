using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AspNetCore.MultiTenancy
{
    [DependsOn(
        typeof(AbpMultiTenancyAbstractionsModule), 
        typeof(AbpAspNetCoreModule)
        )]
    public class AbpAspNetCoreMultiTenancyModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<TenantResolveOptions>(options =>
            {
                options.TenantResolvers.Add(new QueryStringTenantResolveContributer());
                options.TenantResolvers.Add(new RouteTenantResolveContributer());
                options.TenantResolvers.Add(new HeaderTenantResolveContributer());
                options.TenantResolvers.Add(new CookieTenantResolveContributer());
            });

            services.AddAssemblyOf<AbpAspNetCoreMultiTenancyModule>();
        }
    }
}
