using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace BaseManagement
{
    [DependsOn(
        typeof(BaseManagementDomainModule),
        typeof(BaseManagementApplicationContractsModule),
        typeof(AbpAutoMapperModule)
        )]
    public class BaseManagementApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<BaseManagementApplicationAutoMapperProfile>(validate: true);
            });
        }
    }
}
