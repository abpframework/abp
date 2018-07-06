using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Packages
{
    [DependsOn(typeof(AbpAspNetCoreMvcUiBundlingModule))]
    public class AbpAspNetCoreMvcUiPackagesModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAssemblyOf<AbpAspNetCoreMvcUiPackagesModule>();
        }
    }
}
