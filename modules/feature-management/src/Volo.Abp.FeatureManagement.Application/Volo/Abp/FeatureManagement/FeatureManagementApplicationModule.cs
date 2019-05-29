using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Volo.Abp.FeatureManagement
{
    [DependsOn(
        typeof(FeatureManagementDomainModule),
        typeof(FeatureManagementApplicationContractsModule),
        typeof(AutoMapperModule)
        )]
    public class FeatureManagementApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<FeatureManagementApplicationAutoMapperProfile>(validate: true);
            });
        }
    }
}
