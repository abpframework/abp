using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using Volo.Abp.SettingManagement.Localization;

namespace Volo.Abp.SettingManagement
{
    [DependsOn(typeof(AbpLocalizationModule))]
    public class AbpSettingManagementDomainSharedModule : AbpModule
    {

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<AbpSettingManagementResource>("en");
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<AbpSettingManagementResource>()
                    .AddVirtualJson("/Volo/Abp/SettingManagement/Localization/Resources/AbpSettingManagement");
            });
        }
    }
}
