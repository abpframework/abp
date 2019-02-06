using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace Volo.Abp.BackgroundJobs.MongoDB
{
    [DependsOn(
        typeof(BackgroundJobsDomainModule),
        typeof(AbpMongoDbModule)
        )]
    public class BackgroundJobsMongoDbModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMongoDbContext<BackgroundJobsMongoDbContext>(options =>
            {
                 options.AddRepository<BackgroundJobRecord, MongoBackgroundJobRepository>();
            });
        }
    }
}
