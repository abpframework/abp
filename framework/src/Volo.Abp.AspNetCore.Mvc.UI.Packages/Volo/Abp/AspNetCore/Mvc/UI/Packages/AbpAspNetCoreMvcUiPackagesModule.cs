using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Packages
{
    [DependsOn(typeof(AbpAspNetCoreMvcUiBundlingModule))]
    public class AbpAspNetCoreMvcUiPackagesModule : AbpModule
    {

    }
}
