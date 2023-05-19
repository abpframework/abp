using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.TenantManagement;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class AbpTenantManagementInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpTenantManagementInstallerModule>();
        });
    }
}
