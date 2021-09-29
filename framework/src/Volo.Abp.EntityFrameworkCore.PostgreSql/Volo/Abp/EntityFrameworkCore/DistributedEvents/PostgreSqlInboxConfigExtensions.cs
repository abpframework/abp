using Volo.Abp.EventBus.Distributed;

namespace Volo.Abp.EntityFrameworkCore.DistributedEvents
{
    public static class PostgreSqlInboxConfigExtensions
    {
        public static void UsePostgreSql<TDbContext>(this InboxConfig outboxConfig)
            where TDbContext : IHasEventInbox
        {
            outboxConfig.ImplementationType = typeof(IPostgreSqlDbContextEventInbox<TDbContext>);
        }
    }
}
