using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Dashboards
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcUiWidgetsModule)
        )]
    public class AbpAspNetCoreMvcUiDashboardsModule : AbpModule
    {

    }
}
