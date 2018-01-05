using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace Volo.Abp.MultiTenancy
{
    [DependsOn(typeof(AbpDataModule))]
    [DependsOn(typeof(AbpMultiTenancyAbstractionsModule))]
    [DependsOn(typeof(AbpDddModule))]
    public class AbpMultiTenancyDomainModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpMultiTenancyDomainModule>();
        }
    }
}
