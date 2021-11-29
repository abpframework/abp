using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Volo.Abp.BackgroundJobs.EntityFrameworkCore
{
    [DependsOn(
        typeof(AbpBackgroundJobsDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class AbpBackgroundJobsEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<BackgroundJobsDbContext>(options =>
            {
                 options.AddRepository<BackgroundJobRecord, EfCoreBackgroundJobRepository>();
            });
        }
    }
}