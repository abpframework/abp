using Volo.Abp.Data;
using Volo.Abp.EventBus.Distributed;

namespace Volo.Abp.MongoDB.DistributedEvents;

public static class MongoDbInboxConfigExtensions
{
    public static void UseMongoDbContext<TMongoDbContext>(this InboxConfig outboxConfig)
        where TMongoDbContext : IHasEventInbox
    {
        outboxConfig.ImplementationType = typeof(IMongoDbContextEventInbox<TMongoDbContext>);
        outboxConfig.DatabaseName = ConnectionStringNameAttribute.GetConnStringName<TMongoDbContext>();
    }
}
