using System;
using Mongo2Go;
using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace Acme.BookStore.MongoDB
{
    [DependsOn(
        typeof(BookStoreTestBaseModule),
        typeof(BookStoreMongoDbModule)
        )]
    public class BookStoreMongoDbTestModule : AbpModule
    {
        private static readonly MongoDbRunner MongoDbRunner = MongoDbRunner.Start();

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var connectionString = MongoDbRunner.ConnectionString.EnsureEndsWith('/') +
                                    "Db_" +		
                                    Guid.NewGuid().ToString("N");

            Configure<AbpDbConnectionOptions>(options =>
            {
                options.ConnectionStrings.Default = connectionString;
            });
        }
    }
}