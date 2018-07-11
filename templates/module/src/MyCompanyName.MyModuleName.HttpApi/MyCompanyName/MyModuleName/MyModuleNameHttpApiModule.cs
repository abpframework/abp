using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace MyCompanyName.MyModuleName
{
    [DependsOn(
        typeof(MyModuleNameApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class MyModuleNameHttpApiModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAssemblyOf<MyModuleNameHttpApiModule>();
        }
    }
}
