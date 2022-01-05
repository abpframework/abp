using Volo.Abp.EventBus.Distributed;

namespace Volo.Abp.EntityFrameworkCore.DistributedEvents;

public static class PostgreSqlOutboxConfigExtensions
{
    public static void UseNpgsql<TDbContext>(this OutboxConfig outboxConfig)
        where TDbContext : IHasEventOutbox
    {
        outboxConfig.ImplementationType = typeof(IPostgreSqlDbContextEventOutbox<TDbContext>);
    }
}
