using Volo.Abp.FeatureManagement.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.Resources.AbpValidation;
using Volo.Abp.Modularity;

namespace Volo.Abp.FeatureManagement
{
    [DependsOn(
        typeof(AbpLocalizationModule)
        )]
    public class AbpFeatureManagementDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources.Add<AbpFeatureManagementResource>("en");
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<AbpFeatureManagementResource>()
                    .AddBaseTypes(
                        typeof(AbpValidationResource)
                    ).AddVirtualJson("Volo/Abp/FeatureManagement/Localization/Resources/Domain");
            });
        }
    }
}
