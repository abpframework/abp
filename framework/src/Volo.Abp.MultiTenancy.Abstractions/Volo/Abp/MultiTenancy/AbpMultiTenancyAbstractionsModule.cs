using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.MultiTenancy;

[DependsOn(
    typeof(AbpVirtualFileSystemModule),
    typeof(AbpLocalizationModule)
)]
public class AbpMultiTenancyAbstractionsModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpMultiTenancyAbstractionsModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<AbpMultiTenancyResource>("en")
                .AddVirtualJson("/Volo/Abp/MultiTenancy/Localization");
        });
    }
}
