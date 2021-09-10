using Volo.Abp.EventBus.Distributed;

namespace Volo.Abp.MongoDB.DistributedEvents
{
    public interface IDbContextEventOutbox<TDbContext> : IEventOutbox
        where TDbContext : IHasEventOutbox
    {
        
    }
}