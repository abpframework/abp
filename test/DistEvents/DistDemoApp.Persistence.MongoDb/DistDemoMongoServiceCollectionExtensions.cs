using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Data;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;
using Volo.Abp.MongoDB.DistributedEvents;

namespace DistDemoApp;

public static class DistDemoMongoServiceCollectionExtensions
{
    private const string DefaultMongoConnectionString = "mongodb://localhost:27017/DistEventsDemo?retryWrites=false";

    public static void ConfigureDistDemoMongoInfrastructure(this ServiceConfigurationContext context)
    {
        context.Services.AddMongoDbContext<DistDemoMongoDbContext>(options =>
        {
            options.AddDefaultRepositories();
        });

        context.Services.Configure<AbpDbConnectionOptions>(options =>
        {
            if (string.IsNullOrWhiteSpace(options.ConnectionStrings.Default))
            {
                options.ConnectionStrings.Default = DefaultMongoConnectionString;
            }
        });

        context.Services.Configure<AbpDistributedEventBusOptions>(options =>
        {
            options.Outboxes.Configure(config =>
            {
                config.UseMongoDbContext<DistDemoMongoDbContext>();
            });

            options.Inboxes.Configure(config =>
            {
                config.UseMongoDbContext<DistDemoMongoDbContext>();
            });
        });
    }
}
