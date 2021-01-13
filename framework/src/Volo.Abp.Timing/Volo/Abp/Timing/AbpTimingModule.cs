using Volo.Abp.Localization;
using Volo.Abp.Localization.Resources.AbpLocalization;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;
using Volo.Abp.Timing.Localization.Resources.AbpTiming;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Timing
{
    [DependsOn(
        typeof(AbpLocalizationModule),
        typeof(AbpSettingsModule)
        )]
    public class AbpTimingModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpTimingModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options
                    .Resources
                    .Add<AbpTimingResource>("en")
                    .AddVirtualJson("/Volo/Abp/Timing/Localization");
            });
        }
    }
}
