using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.MultiTenancy
{
    [DependsOn(typeof(AbpDddModule))]
    [DependsOn(typeof(AbpMultiTenancyDomainSharedModule))]
    public class AbpMultiTenancyApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpMultiTenancyApplicationContractsModule>();
        }
    }
}