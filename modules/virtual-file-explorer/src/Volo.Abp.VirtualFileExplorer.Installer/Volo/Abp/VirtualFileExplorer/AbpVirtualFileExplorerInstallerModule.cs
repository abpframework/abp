using Volo.Abp.Modularity;
using Volo.Abp.Studio;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.VirtualFileExplorer
{
    [DependsOn(
        typeof(AbpStudioModuleInstallerModule),
        typeof(AbpVirtualFileSystemModule)
        )]
    public class AbpVirtualFileExplorerInstallerModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpVirtualFileExplorerInstallerModule>();
            });
        }
    }
}
