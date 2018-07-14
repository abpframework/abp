using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using MyCompanyName.MyProjectName.Localization;

namespace MyCompanyName.MyProjectName
{
    [DependsOn(
        typeof(AbpLocalizationModule)
        )]
    public class MyProjectNameDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources.Add<MyProjectNameResource>("en");
            });

            context.Services.AddAssemblyOf<MyProjectNameDomainSharedModule>();
        }
    }
}
