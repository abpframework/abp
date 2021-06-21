using System;
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
            var stringArray = MyProjectNameMongoDbFixture.ConnectionString.Split('?');
                        var connectionString = stringArray[0].EnsureEndsWith('/')  +
                                                   "Db_" +
                                               Guid.NewGuid().ToString("N") + "/?" + stringArray[1];

            Configure<AbpDbConnectionOptions>(options =>
            {
                options.ConnectionStrings.Default = connectionString;
            });
        }
    }
}
