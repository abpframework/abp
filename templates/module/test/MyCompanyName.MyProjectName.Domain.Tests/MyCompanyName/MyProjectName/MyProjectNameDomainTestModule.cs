using Microsoft.Extensions.DependencyInjection;
using MyCompanyName.MyProjectName.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace MyCompanyName.MyProjectName
{
    [DependsOn(
        typeof(MyProjectNameEntityFrameworkCoreTestModule)
        )]
    public class MyProjectNameDomainTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAssemblyOf<MyProjectNameDomainTestModule>();
        }
    }
}
