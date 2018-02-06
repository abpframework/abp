using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Settings
{
    [DependsOn(typeof(AbpSettingsModule))]
    [DependsOn(typeof(AbpDddModule))]
    [DependsOn(typeof(AbpMultiTenancyAbstractionsModule))]
    public class AbpSettingsDomainModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpSettingsDomainModule>();
        }
    }
}
