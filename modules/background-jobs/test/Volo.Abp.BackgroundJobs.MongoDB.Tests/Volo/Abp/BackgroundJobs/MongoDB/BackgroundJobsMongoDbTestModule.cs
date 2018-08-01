using Microsoft.Extensions.DependencyInjection;
using Mongo2Go;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace Volo.Abp.BackgroundJobs.MongoDB
{
    [DependsOn(
        typeof(BackgroundJobsTestBaseModule),
        typeof(BackgroundJobsMongoDbModule)
        )]
    public class BackgroundJobsMongoDbTestModule : AbpModule
    {
        private MongoDbRunner _mongoDbRunner;

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            _mongoDbRunner = MongoDbRunner.Start();

            context.Services.Configure<DbConnectionOptions>(options =>
            {
                options.ConnectionStrings.Default = _mongoDbRunner.ConnectionString;
            });

            context.Services.AddAssemblyOf<BackgroundJobsMongoDbTestModule>();
        }

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            _mongoDbRunner.Dispose();
        }
    }
}