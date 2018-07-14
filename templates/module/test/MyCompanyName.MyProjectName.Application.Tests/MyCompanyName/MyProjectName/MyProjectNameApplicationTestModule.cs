using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace MyCompanyName.MyProjectName
{
    [DependsOn(
        typeof(MyProjectNameApplicationModule),
        typeof(MyProjectNameDomainTestModule)
        )]
    public class MyProjectNameApplicationTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAssemblyOf<MyProjectNameApplicationTestModule>();
        }
    }
}
