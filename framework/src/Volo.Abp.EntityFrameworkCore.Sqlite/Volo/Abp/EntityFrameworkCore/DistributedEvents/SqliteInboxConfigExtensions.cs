using Volo.Abp.EventBus.Distributed;

namespace Volo.Abp.EntityFrameworkCore.DistributedEvents
{
    public static class SqliteInboxConfigExtensions
    {
        public static void UseSqlite<TDbContext>(this InboxConfig outboxConfig)
            where TDbContext : IHasEventInbox
        {
            outboxConfig.ImplementationType = typeof(ISqlRawDbContextEventInbox<TDbContext>);
        }
    }
}
