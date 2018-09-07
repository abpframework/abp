using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.TenantSwitch
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcUiThemeSharedModule),
        typeof(AbpAspNetCoreMultiTenancyModule)
        )]
    public class AbpAspNetCoreMvcUiTenantSwitchModule : AbpModule
    {

    }
}
