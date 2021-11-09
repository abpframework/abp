using Localization.Resources.AbpUi;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.ExceptionHandling.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.UI;

[DependsOn(
    typeof(AbpExceptionHandlingModule)
)]
public class AbpUiModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpUiModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<AbpUiResource>("en")
                .AddBaseTypes(typeof(AbpExceptionHandlingResource))
                .AddVirtualJson("/Localization/Resources/AbpUi");
        });
    }
}
