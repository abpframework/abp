using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Demo
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcUiThemeSharedModule)
        )]
    public class AbpAspNetCoreMvcUiThemeSharedDemoModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpAspNetCoreMvcUiThemeSharedDemoModule>();
            });
        }
    }
}
