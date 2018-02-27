using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace Volo.Abp.MultiTenancy
{
    [DependsOn(typeof(AbpMultiTenancyApplicationContractsModule))]
    [DependsOn(typeof(AbpAspNetCoreMvcModule))]
    public class AbpMultiTenancyHttpApiProxyModule : AbpModule
    {
        public override void PreConfigureServices(IServiceCollection services)
        {
            services.PreConfigure<IMvcCoreBuilder>(options =>
            {
                options.PartManager.ApplicationParts.Add(new AssemblyPart(typeof(AbpMultiTenancyHttpApiProxyModule).Assembly));
            });
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpMultiTenancyHttpApiProxyModule>();
        }
    }
}