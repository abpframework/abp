using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.EntityFrameworkCore.MySQL
{
    [DependsOn(
        typeof(AbpEntityFrameworkCoreModule)
        )]
    public class AbpEntityFrameworkCoreMySQLModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpEntityFrameworkCoreMySQLModule>();
        }
    }
}
