using Volo.Abp.Caching;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;

namespace Volo.Abp.SettingManagement
{
    [DependsOn(
        typeof(SettingsModule),
        typeof(DddDomainModule),
        typeof(SettingManagementDomainSharedModule), 
        typeof(CachingModule)
        )]
    public class SettingManagementDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<SettingManagementOptions>(options =>
            {
                options.Providers.Add<DefaultValueSettingManagementProvider>();
                options.Providers.Add<GlobalSettingManagementProvider>();
                options.Providers.Add<TenantSettingManagementProvider>();
                options.Providers.Add<UserSettingManagementProvider>();
            });
        }
    }
}
