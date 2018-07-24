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
            BackgroundJobsBsonClassMap.Configure();

            context.Services.AddMongoDbContext<BackgroundJobsMongoDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, MongoQuestionRepository>();
                 */
            });

            context.Services.AddAssemblyOf<BackgroundJobsMongoDbModule>();
        }
    }
}
