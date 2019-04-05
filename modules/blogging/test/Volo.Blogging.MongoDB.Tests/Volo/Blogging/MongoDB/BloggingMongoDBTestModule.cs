using Mongo2Go;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Blogging.MongoDB;

namespace Volo.Blogging
{
    [DependsOn(
        typeof(BloggingTestBaseModule),
        typeof(BloggingMongoDbModule)
    )]
    public class BloggingMongoDBTestModule : AbpModule
    {
        private MongoDbRunner _mongoDbRunner;

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            _mongoDbRunner = MongoDbRunner.Start();

            Configure<DbConnectionOptions>(options =>
            {
                options.ConnectionStrings.Default = _mongoDbRunner.ConnectionString;
            });
        }

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            _mongoDbRunner.Dispose();
        }
    }
}