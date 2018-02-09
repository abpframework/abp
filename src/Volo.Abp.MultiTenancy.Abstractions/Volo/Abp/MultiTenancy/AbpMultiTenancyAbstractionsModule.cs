using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;

namespace Volo.Abp.MultiTenancy
{
    [DependsOn(typeof(AbpDataModule))]
    [DependsOn(typeof(AbpSettingsModule))]
    public class AbpMultiTenancyAbstractionsModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<SettingOptions>(options =>
            {
                options.ValueProviders.Add<TenantSettingValueProvider>();
            });

            services.AddAssemblyOf<AbpMultiTenancyAbstractionsModule>();
        }
    }
}
