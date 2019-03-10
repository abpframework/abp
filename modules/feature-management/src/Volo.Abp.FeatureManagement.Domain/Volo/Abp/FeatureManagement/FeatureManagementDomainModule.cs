using Microsoft.Extensions.DependencyInjection;
using Abp.FeatureManagement.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Abp.FeatureManagement
{
    [DependsOn(
        typeof(FeatureManagementDomainSharedModule)
        )]
    public class FeatureManagementDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<FeatureManagementDomainModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources.Get<FeatureManagementResource>().AddVirtualJson("/Abp/FeatureManagement/Localization/Domain");
            });

            Configure<ExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("FeatureManagement", typeof(FeatureManagementResource));
            });
        }
    }
}
