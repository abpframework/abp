using Volo.Abp.AspNetCore.Mvc.Libs;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.Minify;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling;

[DependsOn(
    typeof(AbpAspNetCoreMvcUiBootstrapModule),
    typeof(AbpMinifyModule),
    typeof(AbpAspNetCoreMvcUiBundlingAbstractionsModule)
    )]
public class AbpAspNetCoreMvcUiBundlingModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpMvcLibsOptions>(options =>
        {
            options.CheckLibs = true;
        });
    }
}
