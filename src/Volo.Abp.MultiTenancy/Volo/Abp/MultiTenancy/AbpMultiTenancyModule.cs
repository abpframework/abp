using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace Volo.Abp.MultiTenancy
{
    [DependsOn(typeof(AbpDataModule))]
    [DependsOn(typeof(AbpMultiTenancyAbstractionsModule))]
    public class AbpMultiTenancyModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpMultiTenancyModule>();
        }
    }
}
