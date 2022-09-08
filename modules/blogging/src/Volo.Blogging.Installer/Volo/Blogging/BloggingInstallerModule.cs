using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Blogging;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class BloggingInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<BloggingInstallerModule>();
        });
    }
}
