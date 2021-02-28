using Volo.Abp.Emailing;
using Volo.Abp.Modularity;

namespace Volo.Abp.SettingManagement
{
    [DependsOn(
        typeof(AbpSettingManagementDomainModule),
        typeof(AbpSettingManagementApplicationContractsModule),
        typeof(AbpSettingManagementDomainModule),
        typeof(AbpEmailingModule)
    )]
    public class AbpSettingManagementApplicationModule : AbpModule
    {
    }
}
