using Volo.Abp.EventBus.Distributed;

namespace Volo.Abp.MongoDB.DistributedEvents
{
    public interface IDbContextEventInbox<TDbContext> : IEventInbox
        where TDbContext : IHasEventInbox
    {
        
    }
}