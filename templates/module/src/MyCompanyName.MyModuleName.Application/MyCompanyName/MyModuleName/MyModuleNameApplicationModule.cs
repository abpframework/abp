using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;

namespace MyCompanyName.MyModuleName
{
    [DependsOn(
        typeof(MyModuleNameDomainModule),
        typeof(MyModuleNameApplicationContractsModule),
        typeof(AbpAutoMapperModule)
        )]
    public class MyModuleNameApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<MyModuleNameApplicationAutoMapperProfile>(validate: true);
            });

            context.Services.Configure<SettingOptions>(options =>
            {
                options.DefinitionProviders.Add<MyModuleNameSettingDefinitionProvider>();
            });

            context.Services.AddAssemblyOf<MyModuleNameApplicationModule>();
        }
    }
}
