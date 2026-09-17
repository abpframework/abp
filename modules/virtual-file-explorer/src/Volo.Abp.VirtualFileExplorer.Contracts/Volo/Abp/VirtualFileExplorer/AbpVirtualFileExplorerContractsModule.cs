using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileExplorer.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.VirtualFileExplorer;

public class AbpVirtualFileExplorerContractsModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpVirtualFileExplorerContractsModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<VirtualFileExplorerResource>("en")
                .AddVirtualJson("/Volo/Abp/VirtualFileExplorer/Localization/Resources");
        });
    }
}
