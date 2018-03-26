using Microsoft.Extensions.DependencyInjection;
using Mongo2Go;
using Volo.Abp.Autofac;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.TestApp;
using Volo.Abp.TestApp.MongoDb;

namespace Volo.Abp.MongoDB
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpMongoDbModule),
        typeof(AbpTestBaseModule),
        typeof(TestAppModule)
        )]
    public class AbpMongoDbTestModule : AbpModule
    {
        private MongoDbRunner _mongoDbRunner;

        public override void ConfigureServices(IServiceCollection services)
        {
            _mongoDbRunner = MongoDbRunner.Start();

            services.Configure<DbConnectionOptions>(options =>
            {
                options.ConnectionStrings.Default = _mongoDbRunner.ConnectionString;
            });

            services.AddMongoDbContext<TestAppMongoDbContext>(options =>
            {
                options.AddDefaultRepositories<ITestAppMongoDbContext>();
            });

            services.AddAssemblyOf<AbpMongoDbTestModule>();
        }

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            _mongoDbRunner.Dispose();
        }
    }
}
