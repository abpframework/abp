using Volo.Abp.EventBus.Distributed;

namespace Volo.Abp.MongoDB.DistributedEvents
{
    public static class MongoDbOutboxConfigExtensions
    {
        public static void UseMongoDbContext<TMongoDbContext>(this OutboxConfig outboxConfig)
            where TMongoDbContext : IHasEventOutbox
        {
            outboxConfig.ImplementationType = typeof(IMongoDbContextEventOutbox<TMongoDbContext>);
        }
    }
}