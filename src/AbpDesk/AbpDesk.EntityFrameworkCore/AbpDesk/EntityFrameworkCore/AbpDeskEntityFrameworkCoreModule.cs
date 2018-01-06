using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy.EntityFrameworkCore;

namespace AbpDesk.EntityFrameworkCore
{
    [DependsOn(
        typeof(AbpDeskDomainModule),
        typeof(AbpEntityFrameworkCoreModule),
        typeof(AbpMultiTenancyEntityFrameworkCoreModule)
        )]
    public class AbpDeskEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAbpDbContext<AbpDeskDbContext>(options =>
            {
                options.AddDefaultRepositories();
                options.ReplaceDbContext<IMultiTenancyDbContext>();
            });

            services.AddAssemblyOf<AbpDeskEntityFrameworkCoreModule>();
        }
    }
}
