using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Users;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class AbpUsersInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpUsersInstallerModule>();
        });
    }
}
