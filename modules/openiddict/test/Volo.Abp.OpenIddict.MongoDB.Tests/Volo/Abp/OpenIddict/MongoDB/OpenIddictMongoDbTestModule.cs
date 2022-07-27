using System;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.Identity.MongoDB;
using Volo.Abp.MongoDB;
using Volo.Abp.Uow;

namespace Volo.Abp.OpenIddict.MongoDB;

[DependsOn(
    typeof(OpenIddictTestBaseModule),
    typeof(AbpIdentityMongoDbModule),
    typeof(AbpOpenIddictMongoDbModule)
    )]
public class OpenIddictMongoDbTestModule : AbpModule
{
    private static string _connectionString;
    
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var stringArray = MongoDbFixture.ConnectionString.Split('?');
        _connectionString = stringArray[0].EnsureEndsWith('/') +
                           "Db_" +
                           Guid.NewGuid().ToString("N") + "/?" + stringArray[1];

        Configure<AbpDbConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default = _connectionString;
        });
    }

    public override void OnPreApplicationInitialization(ApplicationInitializationContext context)
    {
        Migrate(context);
    }

    private static void Migrate(ApplicationInitializationContext context)
    {
        var dbContexts = context.ServiceProvider.GetServices<IAbpMongoDbContext>();

        foreach (var dbContext in dbContexts)
        {
            var mongoUrl = new MongoUrl(_connectionString);
            var databaseName = mongoUrl.DatabaseName;
            var client = new MongoClient(mongoUrl);

            if (databaseName.IsNullOrWhiteSpace())
            {
                databaseName = ConnectionStringNameAttribute.GetConnStringName(dbContext.GetType());
            }

            (dbContext as AbpMongoDbContext)?.InitializeCollections(client.GetDatabase(databaseName));
        }
    }
}
