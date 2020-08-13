using Volo.Abp.Application;
using Volo.Abp.Json.Newtonsoft;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.FeatureManagement
{
    [DependsOn(
        typeof(AbpFeatureManagementDomainSharedModule),
        typeof(AbpDddApplicationModule)
        )]
    public class AbpFeatureManagementApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpFeatureManagementApplicationContractsModule>();
            });

            Configure<AbpNewtonsoftJsonSerializerOptions>(options =>
            {
                options.Converters.Add<StringValueTypeJsonConverter>();
            });
        }
    }
}
