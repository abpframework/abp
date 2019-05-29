using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.Localization;

namespace Volo.Abp.PermissionManagement
{
    [DependsOn(
        typeof(LocalizationModule)
        )]
    public class PermissionManagementDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<AbpPermissionManagementResource>("en");
            });
        }
    }
}
