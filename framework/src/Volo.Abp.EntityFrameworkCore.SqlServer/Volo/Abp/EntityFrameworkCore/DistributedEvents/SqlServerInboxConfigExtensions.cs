using Volo.Abp.EventBus.Distributed;

namespace Volo.Abp.EntityFrameworkCore.DistributedEvents;

public static class SqlServerInboxConfigExtensions
{
    public static void UseSqlServer<TDbContext>(this InboxConfig outboxConfig)
        where TDbContext : IHasEventInbox
    {
        outboxConfig.ImplementationType = typeof(ISqlRawDbContextEventInbox<TDbContext>);
    }
}
