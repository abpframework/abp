using OrganizationService.Localization;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace OrganizationService
{
    /* This module directly depends on EF Core by its design.
     * In this way, we can directly use EF Core async LINQ extension methods.
     */
    [DependsOn(
        typeof(OrganizationServiceDomainSharedModule),
        typeof(AbpEntityFrameworkCoreModule) 
        )]
    public class OrganizationServiceDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<OrganizationServiceDomainModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources.Get<OrganizationServiceResource>().AddVirtualJson("/OrganizationService/Localization/Domain");
            });

            Configure<ExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("OrganizationService", typeof(OrganizationServiceResource));
            });
        }
    }
}
