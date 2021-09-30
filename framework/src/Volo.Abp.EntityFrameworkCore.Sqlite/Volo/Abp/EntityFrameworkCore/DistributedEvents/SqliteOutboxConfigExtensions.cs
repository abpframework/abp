using Volo.Abp.EventBus.Distributed;

namespace Volo.Abp.EntityFrameworkCore.DistributedEvents
{
    public static class SqliteOutboxConfigExtensions
    {
        public static void UseSqlite<TDbContext>(this OutboxConfig outboxConfig)
            where TDbContext : IHasEventOutbox
        {
            outboxConfig.ImplementationType = typeof(ISqlRawDbContextEventOutbox<TDbContext>);
        }
    }
}
