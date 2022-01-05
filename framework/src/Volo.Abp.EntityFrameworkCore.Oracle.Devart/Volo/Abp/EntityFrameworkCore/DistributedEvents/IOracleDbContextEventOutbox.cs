namespace Volo.Abp.EntityFrameworkCore.DistributedEvents;

public interface IOracleDbContextEventOutbox<TDbContext> : IDbContextEventOutbox<TDbContext>
    where TDbContext : IHasEventOutbox
{
}
