using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace MyCompanyName.MyProjectName
{
    [DependsOn(
        typeof(MyProjectNameDomainModule),
        typeof(MyProjectNameApplicationContractsModule),
        typeof(AbpAutoMapperModule)
        )]
    public class MyProjectNameApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                //Adds all profiles in the MyProjectNameApplicationModule assembly by validating configurations
                options.AddMaps<MyProjectNameApplicationModule>(validate: true);

                //Exclude a profile from the configuration validation
                options.ValidateProfile<MyProjectNameApplicationAutoMapperProfile>(validate: false);
            });
        }
    }
}
