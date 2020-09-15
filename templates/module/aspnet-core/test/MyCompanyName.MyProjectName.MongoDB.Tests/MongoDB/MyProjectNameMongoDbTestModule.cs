using System;
using System.Linq;
using System.Threading;
using MongoDB.Driver;
using MongoDB.Driver.Core.Servers;
using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace MyCompanyName.MyProjectName.MongoDB
{
    [DependsOn(
        typeof(MyProjectNameTestBaseModule),
        typeof(MyProjectNameMongoDbModule)
        )]
    public class MyProjectNameMongoDbTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var stringArray = MongoDbFixture.ConnectionString.Split('?');

            var connectionString = stringArray[0].EnsureEndsWith('/') +
                                   "Db_" +
                                   Guid.NewGuid().ToString("N") + "/?" + stringArray[1];

            Configure<AbpDbConnectionOptions>(options =>
            {
                options.ConnectionStrings.Default = connectionString;
            });

            EnsureTransactionIsReady(new MongoClient(connectionString));
        }

        private void EnsureTransactionIsReady(MongoClient client)
        {
            SpinWait.SpinUntil(() => client.Cluster.Description.Servers.Any(s => s.State == ServerState.Connected && s.IsDataBearing));
        }
    }
}
