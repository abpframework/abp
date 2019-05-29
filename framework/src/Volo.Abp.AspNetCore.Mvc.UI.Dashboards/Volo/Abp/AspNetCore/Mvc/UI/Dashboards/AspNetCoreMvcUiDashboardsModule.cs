using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.AspNetCore.Mvc.UI.Dashboards
{
    [DependsOn(
        typeof(AspNetCoreMvcUiWidgetsModule)
    )]
    public class AspNetCoreMvcUiDashboardsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AspNetCoreMvcUiDashboardsModule>();
            });
        }
    }
}
