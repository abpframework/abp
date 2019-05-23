using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Widgets
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcUiBootstrapModule)
        )]
    public class AbpAspNetCoreMvcUiWidgetsModule : AbpModule
    {

    }
}
