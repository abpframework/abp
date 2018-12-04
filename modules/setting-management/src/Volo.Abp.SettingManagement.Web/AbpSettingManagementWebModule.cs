using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.Modularity;

namespace Volo.Abp.SettingManagement.Web
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcUiThemeSharedModule)
        )]
    public class AbpSettingManagementWebModule : AbpModule
    {

    }
}
