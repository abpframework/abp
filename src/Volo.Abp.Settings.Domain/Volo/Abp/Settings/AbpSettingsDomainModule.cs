using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace Volo.Abp.Settings
{
    [DependsOn(typeof(AbpSettingsModule))]
    [DependsOn(typeof(AbpDddApplicationModule))]
    [DependsOn(typeof(AbpSettingsDomainSharedModule))]
    public class AbpSettingsDomainModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpSettingsDomainModule>();
        }
    }
}
