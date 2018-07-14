using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace MyCompanyName.MyProjectName
{
    [DependsOn(
        typeof(MyProjectNameApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class MyProjectNameHttpApiModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAssemblyOf<MyProjectNameHttpApiModule>();
        }
    }
}
