using Volo.Abp.EventBus.Distributed;

namespace Volo.Abp.EntityFrameworkCore.DistributedEvents
{
    public static class EfCoreInboxConfigExtensions
    {
        public static void UseDbContext<TDbContext>(this InboxConfig outboxConfig)
            where TDbContext : IHasEventInbox
        {
            outboxConfig.ImplementationType = typeof(IDbContextEventInbox<TDbContext>);
        }
    }
}