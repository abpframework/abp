using BaseManagement.Localization;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace BaseManagement
{
    /* This module directly depends on EF Core by its design.
     * In this way, we can directly use EF Core async LINQ extension methods.
     */
    [DependsOn(
        typeof(BaseManagementDomainSharedModule),
        typeof(AbpEntityFrameworkCoreModule) 
        )]
    public class BaseManagementDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<BaseManagementDomainModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources.Get<BaseManagementResource>().AddVirtualJson("/BaseManagement/Localization/Domain");
            });

            Configure<ExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("BaseManagement", typeof(BaseManagementResource));
            });
        }
    }
}
