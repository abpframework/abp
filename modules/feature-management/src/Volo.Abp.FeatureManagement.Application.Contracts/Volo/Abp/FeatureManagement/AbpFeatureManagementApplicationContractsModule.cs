using Volo.Abp.Application;
using Volo.Abp.FeatureManagement.JsonConverters;
using Volo.Abp.Json;
using Volo.Abp.Json.Microsoft;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.FeatureManagement
{
    [DependsOn(
        typeof(AbpFeatureManagementDomainSharedModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpJsonModule)
        )]
    public class AbpFeatureManagementApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpFeatureManagementApplicationContractsModule>();
            });

            Configure<AbpJsonSerializerOptions>(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new StringValueTypeJsonConverter());
            });
        }
    }
}
