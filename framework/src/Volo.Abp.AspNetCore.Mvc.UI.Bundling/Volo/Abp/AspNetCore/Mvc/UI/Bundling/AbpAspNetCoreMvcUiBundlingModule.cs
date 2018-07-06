using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    [DependsOn(typeof(AbpAspNetCoreMvcUiBootstrapModule))]
    public class AbpAspNetCoreMvcUiBundlingModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAssemblyOf<AbpAspNetCoreMvcUiBundlingModule>();
        }
    }
}
