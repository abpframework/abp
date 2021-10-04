using Volo.Abp.EventBus.Distributed;

namespace Volo.Abp.EntityFrameworkCore.DistributedEvents
{
    public interface IDbContextEventOutbox<TDbContext> : IEventOutbox
        where TDbContext : IHasEventOutbox
    {
        
    }
}