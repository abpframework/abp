using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.IdentityServer;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class AbpIdentityServerInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpIdentityServerInstallerModule>();
        });
    }
}
