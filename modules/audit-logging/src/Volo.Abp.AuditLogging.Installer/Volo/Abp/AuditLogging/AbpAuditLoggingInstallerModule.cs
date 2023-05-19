using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.AuditLogging;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class AbpAuditLoggingInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpAuditLoggingInstallerModule>();
        });
    }
}
