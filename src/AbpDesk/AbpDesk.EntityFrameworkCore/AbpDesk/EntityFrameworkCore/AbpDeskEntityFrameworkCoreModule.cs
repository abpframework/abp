using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace AbpDesk.EntityFrameworkCore
{
    [DependsOn(typeof(AbpDeskDomainModule), typeof(AbpEntityFrameworkCoreModule))]
    public class AbpDeskEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpDeskEntityFrameworkCoreModule>();
            services.AddAbpDbContext<AbpDeskDbContext>();
            services.AddDefaultEfCoreRepositories<AbpDeskDbContext>(); //TODO: Move this into AddAbpDbContext as option
        }
    }
}
