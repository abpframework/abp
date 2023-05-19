using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.VirtualFileExplorer;

[DependsOn(
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
