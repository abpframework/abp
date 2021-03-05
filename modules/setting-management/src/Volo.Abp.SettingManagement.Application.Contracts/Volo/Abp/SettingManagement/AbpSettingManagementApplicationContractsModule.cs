using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace Volo.Abp.SettingManagement
{
    [DependsOn(
        typeof(AbpSettingManagementDomainSharedModule),
        typeof(AbpDddApplicationModule)
    )]
    public class AbpSettingManagementApplicationContractsModule : AbpModule
    {
    }
}


