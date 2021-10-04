using Volo.Abp.EventBus.Distributed;

namespace Volo.Abp.EntityFrameworkCore.DistributedEvents
{
    public static class MySQLOutboxConfigExtensions
    {
        public static void UseMySQL<TDbContext>(this OutboxConfig outboxConfig)
            where TDbContext : IHasEventOutbox
        {
            outboxConfig.ImplementationType = typeof(ISqlRawDbContextEventOutbox<TDbContext>);
        }
    }
}
