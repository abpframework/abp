using Volo.Abp.EventBus.Distributed;

namespace Volo.Abp.EntityFrameworkCore.DistributedEvents
{
    public static class SqlServerOutboxConfigExtensions
    {
        public static void UseSqlServer<TDbContext>(this OutboxConfig outboxConfig)
            where TDbContext : IHasEventOutbox
        {
            outboxConfig.ImplementationType = typeof(ISqlRawDbContextEventOutbox<TDbContext>);
        }
    }
}
