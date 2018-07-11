using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using MyCompanyName.MyModuleName.Localization;

namespace MyCompanyName.MyModuleName
{
    [DependsOn(
        typeof(AbpLocalizationModule)
        )]
    public class MyModuleNameDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources.Add<MyModuleNameResource>("en");
            });

            context.Services.AddAssemblyOf<MyModuleNameDomainSharedModule>();
        }
    }
}
