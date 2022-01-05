namespace Volo.Abp.EntityFrameworkCore.DistributedEvents;

public interface ISqlRawDbContextEventInbox<TDbContext> : IDbContextEventInbox<TDbContext>
    where TDbContext : IHasEventInbox
{
}
