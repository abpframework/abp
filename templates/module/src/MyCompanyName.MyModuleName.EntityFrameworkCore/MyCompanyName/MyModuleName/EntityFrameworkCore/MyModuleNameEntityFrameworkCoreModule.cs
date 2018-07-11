using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace MyCompanyName.MyModuleName.EntityFrameworkCore
{
    [DependsOn(
        typeof(MyModuleNameDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class MyModuleNameEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<MyModuleNameDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
            });

            context.Services.AddAssemblyOf<MyModuleNameEntityFrameworkCoreModule>();
        }
    }
}