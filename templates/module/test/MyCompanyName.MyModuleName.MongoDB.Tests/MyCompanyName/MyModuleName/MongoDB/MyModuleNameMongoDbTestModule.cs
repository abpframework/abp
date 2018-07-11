using Microsoft.Extensions.DependencyInjection;
using Mongo2Go;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace MyCompanyName.MyModuleName.MongoDB
{
    [DependsOn(
        typeof(MyModuleNameTestBaseModule),
        typeof(MyModuleNameMongoDbModule)
        )]
    public class MyModuleNameMongoDbTestModule : AbpModule
    {
        private MongoDbRunner _mongoDbRunner;

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            _mongoDbRunner = MongoDbRunner.Start();

            context.Services.Configure<DbConnectionOptions>(options =>
            {
                options.ConnectionStrings.Default = _mongoDbRunner.ConnectionString;
            });

            context.Services.AddAssemblyOf<MyModuleNameMongoDbTestModule>();
        }

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            _mongoDbRunner.Dispose();
        }
    }
}