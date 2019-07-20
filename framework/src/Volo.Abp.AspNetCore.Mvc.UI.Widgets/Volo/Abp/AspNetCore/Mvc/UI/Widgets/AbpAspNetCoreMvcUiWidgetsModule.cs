using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.AspNetCore.Mvc.UI.Widgets
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcUiBootstrapModule),
        typeof(AbpAspNetCoreMvcUiBundlingModule)
    )]
    public class AbpAspNetCoreMvcUiWidgetsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddTransient<DefaultViewComponentHelper>();

            Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpAspNetCoreMvcUiWidgetsModule>();
            });
        }
    }
}
