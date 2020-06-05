using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.Minify;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    [DependsOn(typeof(AbpAspNetCoreMvcUiBootstrapModule), typeof(AbpMinifyModule))]
    public class AbpAspNetCoreMvcUiBundlingModule : AbpModule
    {

    }
}
