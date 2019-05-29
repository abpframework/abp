using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    [DependsOn(typeof(AspNetCoreMvcUiBootstrapModule))]
    public class AspNetCoreMvcUiBundlingModule : AbpModule
    {

    }
}
