using Volo.Abp.Localization.Resources.AbpLocalization;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Localization;

[DependsOn(
    typeof(AbpVirtualFileSystemModule),
    typeof(AbpSettingsModule),
    typeof(AbpLocalizationAbstractionsModule)
    )]
public class AbpLocalizationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        AbpStringLocalizerFactory.Replace(context.Services);

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpLocalizationModule>("Volo.Abp", "Volo/Abp");
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options
                .Resources
                .Add<DefaultResource>("en");

            options
                .Resources
                .Add<AbpLocalizationResource>("en")
                .AddVirtualJson("/Localization/Resources/AbpLocalization");
        });
    }
}
