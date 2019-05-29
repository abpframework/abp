using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.AspNetCore.Mvc.UI
{
    [DependsOn(typeof(AspNetCoreMvcModule))]
    [DependsOn(typeof(UiNavigationModule))]
    public class AspNetCoreMvcUiModule : AbpModule
    {

    }
}
