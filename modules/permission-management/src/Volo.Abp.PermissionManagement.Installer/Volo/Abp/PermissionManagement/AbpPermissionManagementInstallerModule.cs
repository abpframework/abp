using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.PermissionManagement;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class AbpPermissionManagementInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpPermissionManagementInstallerModule>();
        });
    }
}
