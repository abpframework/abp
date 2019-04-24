using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace OrganizationService
{
    [DependsOn(
        typeof(OrganizationServiceDomainModule),
        typeof(OrganizationServiceApplicationContractsModule),
        typeof(AbpAutoMapperModule)
        )]
    public class OrganizationServiceApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<OrganizationServiceApplicationAutoMapperProfile>(validate: false);
            });
        }
    }
}
