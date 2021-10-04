using Volo.Abp.EventBus.Distributed;

namespace Volo.Abp.EntityFrameworkCore.DistributedEvents
{
    public static class OracleOutboxConfigExtensions
    {
        public static void UseOracle<TDbContext>(this OutboxConfig outboxConfig)
            where TDbContext : IHasEventOutbox
        {
            outboxConfig.ImplementationType = typeof(IOracleDbContextEventOutbox<TDbContext>);
        }
    }
}
