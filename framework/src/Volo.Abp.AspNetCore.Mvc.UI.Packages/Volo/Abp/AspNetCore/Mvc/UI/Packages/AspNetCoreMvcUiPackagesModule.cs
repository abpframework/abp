using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Packages
{
    [DependsOn(typeof(AspNetCoreMvcUiBundlingModule))]
    public class AspNetCoreMvcUiPackagesModule : AbpModule
    {

    }
}
