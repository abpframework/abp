using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace MyCompanyName.MyModuleName
{
    [DependsOn(
        typeof(MyModuleNameApplicationModule),
        typeof(MyModuleNameDomainTestModule)
        )]
    public class MyModuleNameApplicationTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAssemblyOf<MyModuleNameApplicationTestModule>();
        }
    }
}
