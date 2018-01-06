using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Volo.Abp.MultiTenancy.EntityFrameworkCore
{
    [DependsOn(typeof(AbpMultiTenancyDomainModule))]
    [DependsOn(typeof(AbpEntityFrameworkCoreModule))]
    public class AbpMultiTenancyEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAbpDbContext<MultiTenancyDbContext>(options =>
            {
                options.AddDefaultRepositories<IMultiTenancyDbContext>();
            });

            services.AddAssemblyOf<AbpMultiTenancyEntityFrameworkCoreModule>();
        }
    }
}
