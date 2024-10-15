using Volo.Abp.AspNetCore.Mvc.Libs;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.Data;
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
        if (!context.Services.IsDataMigrationEnvironment())
        {
            Configure<AbpMvcLibsOptions>(options =>
            {
                options.CheckLibs = true;
            });
        }
    }
}
