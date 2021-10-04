using Volo.Abp.EventBus.Distributed;

namespace Volo.Abp.MongoDB.DistributedEvents
{
    public interface IMongoDbContextEventOutbox<TDbContext> : IEventOutbox
        where TDbContext : IHasEventOutbox
    {
        
    }
}