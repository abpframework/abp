using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;

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
            context.Services.Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<MyProjectNameApplicationAutoMapperProfile>(validate: true);
            });

            context.Services.Configure<SettingOptions>(options =>
            {
                options.DefinitionProviders.Add<MyProjectNameSettingDefinitionProvider>();
            });

            context.Services.AddAssemblyOf<MyProjectNameApplicationModule>();
        }
    }
}
