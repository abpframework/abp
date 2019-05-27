using Volo.Abp.Localization.Resources.AbpValidation;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Localization
{
    [DependsOn(
        typeof(VirtualFileSystemModule),
        typeof(SettingsModule),
        typeof(LocalizationAbstractionsModule)
        )]
    public class LocalizationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            AbpStringLocalizerFactory.Replace(context.Services);

            Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<LocalizationModule>("Volo.Abp", "Volo/Abp");
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options
                    .Resources
                    .Add<DefaultResource>("en");

                options
                    .Resources
                    .Add<AbpValidationResource>("en")
                    .AddVirtualJson("/Localization/Resources/AbpValidation");
            });
        }
    }
}
