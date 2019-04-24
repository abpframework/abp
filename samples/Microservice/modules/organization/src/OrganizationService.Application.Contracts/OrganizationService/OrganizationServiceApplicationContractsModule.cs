using OrganizationService.Localization;
using Volo.Abp.Application;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;
using Volo.Abp.VirtualFileSystem;

namespace OrganizationService
{
    [DependsOn(
        typeof(OrganizationServiceDomainSharedModule),
        typeof(AbpDddApplicationModule)
        )]
    public class OrganizationServiceApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<OrganizationServiceApplicationContractsModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<OrganizationServiceResource>()
                    .AddVirtualJson("/OrganizationService/Localization/ApplicationContracts");
            });
        }
    }
}
