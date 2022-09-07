using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Docs;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class DocsInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<DocsInstallerModule>();
        });
    }
}
