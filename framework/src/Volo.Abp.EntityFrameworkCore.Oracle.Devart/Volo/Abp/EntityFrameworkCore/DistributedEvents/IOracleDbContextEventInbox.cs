namespace Volo.Abp.EntityFrameworkCore.DistributedEvents
{
    public interface IOracleDbContextEventInbox<TDbContext> : IDbContextEventInbox<TDbContext>
        where TDbContext : IHasEventInbox
    {

    }
}
