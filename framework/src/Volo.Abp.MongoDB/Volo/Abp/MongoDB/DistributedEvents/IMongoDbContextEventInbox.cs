using Volo.Abp.EventBus.Distributed;

namespace Volo.Abp.MongoDB.DistributedEvents
{
    public interface IMongoDbContextEventInbox<TDbContext> : IEventInbox
        where TDbContext : IHasEventInbox
    {
        
    }
}