using Volo.Abp.Domain;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;

namespace Volo.Abp.SettingManagement
{
    [DependsOn(typeof(AbpSettingsModule))]
    [DependsOn(typeof(AbpDddDomainModule))]
    [DependsOn(typeof(AbpSettingManagementDomainSharedModule))]
    public class AbpSettingManagementDomainModule : AbpModule
    {
        
    }
}
