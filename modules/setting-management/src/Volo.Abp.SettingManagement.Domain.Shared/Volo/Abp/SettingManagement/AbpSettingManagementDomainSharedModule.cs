using Volo.Abp.Features;
using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using Volo.Abp.SettingManagement.Localization;
using Volo.Abp.Validation;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.SettingManagement;

[DependsOn(typeof(AbpLocalizationModule),
    typeof(AbpValidationModule),
    typeof(AbpFeaturesModule))]
public class AbpSettingManagementDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpSettingManagementDomainSharedModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<AbpSettingManagementResource>("en")
                .AddVirtualJson("/Volo/Abp/SettingManagement/Localization/Resources/AbpSettingManagement");
        });
    }
}
