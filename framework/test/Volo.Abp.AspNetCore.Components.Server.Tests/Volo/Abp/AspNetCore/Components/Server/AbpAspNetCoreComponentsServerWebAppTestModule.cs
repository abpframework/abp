using Volo.Abp.AspNetCore.Components.Web;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Components.Server;

[DependsOn(typeof(AbpAspNetCoreComponentsServerModule))]
public class AbpAspNetCoreComponentsServerWebAppTestModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<AbpAspNetCoreComponentsWebOptions>(options =>
        {
            options.IsBlazorWebApp = true;
        });
    }
}
