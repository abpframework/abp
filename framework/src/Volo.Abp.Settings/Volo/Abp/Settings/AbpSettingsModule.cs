using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security;

namespace Volo.Abp.Settings
{
    [DependsOn(
        typeof(AbpLocalizationAbstractionsModule),
        typeof(AbpSecurityModule),
        typeof(AbpMultiTenancyAbstractionsModule)
        )]
    public class AbpSettingsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<SettingOptions>(options =>
            {
                options.ValueProviders.Add<DefaultValueSettingValueProvider>();
                options.ValueProviders.Add<GlobalSettingValueProvider>();
                options.ValueProviders.Add<TenantSettingValueProvider>();
                options.ValueProviders.Add<UserSettingValueProvider>();
            });
        }
    }
}
