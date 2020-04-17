using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Volo.Abp.FeatureManagement
{
    [DependsOn(
        typeof(AbpFeatureManagementDomainModule),
        typeof(AbpFeatureManagementApplicationContractsModule),
        typeof(AbpAutoMapperModule)
        )]
    public class AbpFeatureManagementApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<AbpFeatureManagementApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<FeatureManagementApplicationAutoMapperProfile>(validate: true);
            });
        }
    }
}
