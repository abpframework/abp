using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AspNetCore.MultiTenancy
{
    [DependsOn(typeof(AbpMultiTenancyModule), typeof(AbpAspNetCoreModule))]
    public class AbpAspNetMultiTenancyModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<MultiTenancyOptions>(options =>
            {
                options.TenantResolvers.Insert(0, new QueryStringTenantResolver());
                //TODO: Web tenant resolvers:
                // values can be tenant id or name, should be validated first by an abstraction (say, ITenantStore over ITenantManager)!
                //- domain/subdomain (not added by default)
                //- route {tenantId}
                //+ querystring
                //- header
                //- cookie
            });

            services.AddAssemblyOf<AbpAspNetMultiTenancyModule>();
        }
    }
}
