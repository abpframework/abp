using Microsoft.Extensions.DependencyInjection;
using MyCompanyName.MyModuleName.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace MyCompanyName.MyModuleName
{
    [DependsOn(
        typeof(MyModuleNameEntityFrameworkCoreTestModule)
        )]
    public class MyModuleNameDomainTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAssemblyOf<MyModuleNameDomainTestModule>();
        }
    }
}
