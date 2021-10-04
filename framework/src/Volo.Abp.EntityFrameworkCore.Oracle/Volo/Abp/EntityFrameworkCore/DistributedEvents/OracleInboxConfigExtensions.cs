using Volo.Abp.EventBus.Distributed;

namespace Volo.Abp.EntityFrameworkCore.DistributedEvents
{
    public static class OracleInboxConfigExtensions
    {
        public static void UseOracle<TDbContext>(this InboxConfig outboxConfig)
            where TDbContext : IHasEventInbox
        {
            outboxConfig.ImplementationType = typeof(IOracleDbContextEventInbox<TDbContext>);
        }
    }
}
