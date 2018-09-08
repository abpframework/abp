using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;

namespace Volo.Abp.MultiTenancy
{
    [DependsOn(typeof(AbpDataModule))]
    [DependsOn(typeof(AbpSettingsModule))]
    public class AbpMultiTenancyAbstractionsModule : AbpModule //TODO: Rename to AbpMultiTenancyModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Configure<SettingOptions>(options =>
            {
                options.ValueProviders.Add<TenantSettingValueProvider>();
            });
        }
    }
}
