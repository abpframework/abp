using Volo.Abp.EventBus.Distributed;

namespace Volo.Abp.EntityFrameworkCore.DistributedEvents
{
    public static class MySQLInboxConfigExtensions
    {
        public static void UseMySQL<TDbContext>(this InboxConfig outboxConfig)
            where TDbContext : IHasEventInbox
        {
            outboxConfig.ImplementationType = typeof(ISqlRawDbContextEventInbox<TDbContext>);
        }
    }
}
