using Volo.Abp.EventBus.Distributed;

namespace Volo.Abp.EntityFrameworkCore.DistributedEvents
{
    public static class EfCoreOutboxConfigExtensions
    {
        public static void UseDbContext<TDbContext>(this OutboxConfig outboxConfig)
            where TDbContext : IHasEventOutbox
        {
            outboxConfig.ImplementationType = typeof(DbContextEventOutbox<TDbContext>);
        }
    }
}