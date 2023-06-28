using Volo.Abp.Application;
using Volo.Abp.Emailing;
using Volo.Abp.Modularity;
using Volo.Abp.Timing;

namespace Volo.Abp.SettingManagement;

[DependsOn(
    typeof(AbpDddApplicationModule),
    typeof(AbpSettingManagementDomainModule),
    typeof(AbpSettingManagementApplicationContractsModule),
    typeof(AbpEmailingModule),
    typeof(AbpTimingModule)
)]
public class AbpSettingManagementApplicationModule : AbpModule
{
}
