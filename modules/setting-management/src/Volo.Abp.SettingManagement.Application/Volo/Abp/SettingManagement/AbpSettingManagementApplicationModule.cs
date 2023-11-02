using Volo.Abp.Application;
using Volo.Abp.Emailing;
using Volo.Abp.Modularity;
using Volo.Abp.Timing;
using Volo.Abp.Users;

namespace Volo.Abp.SettingManagement;

[DependsOn(
    typeof(AbpDddApplicationModule),
    typeof(AbpSettingManagementDomainModule),
    typeof(AbpSettingManagementApplicationContractsModule),
    typeof(AbpEmailingModule),
    typeof(AbpTimingModule),
    typeof(AbpUsersAbstractionModule)
)]
public class AbpSettingManagementApplicationModule : AbpModule
{
}
