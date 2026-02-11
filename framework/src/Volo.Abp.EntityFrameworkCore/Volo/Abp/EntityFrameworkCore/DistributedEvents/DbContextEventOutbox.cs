using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace Volo.Abp.EntityFrameworkCore.DistributedEvents;

public class DbContextEventOutbox<TDbContext> : IDbContextEventOutbox<TDbContext>
    where TDbContext : IHasEventOutbox
{
    protected IDbContextProvider<TDbContext> DbContextProvider { get; }

    public DbContextEventOutbox(
        IDbContextProvider<TDbContext> dbContextProvider)
    {
        DbContextProvider = dbContextProvider;
    }

    [UnitOfWork]
    public virtual async Task EnqueueAsync(OutgoingEventInfo outgoingEvent)
    {
        var dbContext = (IHasEventOutbox)await DbContextProvider.GetDbContextAsync();
        dbContext.OutgoingEvents.Add(new OutgoingEventRecord(outgoingEvent));
    }

    [UnitOfWork]
    public virtual async Task<List<OutgoingEventInfo>> GetWaitingEventsAsync(int maxCount, Expression<Func<IOutgoingEventInfo, bool>>? filter = null, CancellationToken cancellationToken = default)
    {
        var dbContext = (IHasEventOutbox)await DbContextProvider.GetDbContextAsync();

        Expression<Func<OutgoingEventRecord, bool>>? transformedFilter = null;
        if (filter != null)
        {
            transformedFilter = InboxOutboxFilterExpressionTransformer.Transform<IOutgoingEventInfo, OutgoingEventRecord>(filter)!;
        }

        var outgoingEventRecords = await dbContext
            .OutgoingEvents
            .AsNoTracking()
            .WhereIf(transformedFilter != null, transformedFilter!)
            .OrderBy(x => x.CreationTime)
            .Take(maxCount)
            .ToListAsync(cancellationToken: cancellationToken);

        return outgoingEventRecords
            .Select(x => x.ToOutgoingEventInfo())
            .ToList();
    }

    [UnitOfWork]
    public virtual async Task DeleteAsync(Guid id)
    {
        var dbContext = (IHasEventOutbox)await DbContextProvider.GetDbContextAsync();
        await dbContext.OutgoingEvents.Where(x => x.Id == id).ExecuteDeleteAsync();
    }

    [UnitOfWork]
    public virtual async Task DeleteManyAsync(IEnumerable<Guid> ids)
    {
        var dbContext = (IHasEventOutbox)await DbContextProvider.GetDbContextAsync();
        await dbContext.OutgoingEvents.Where(x => ids.Contains(x.Id)).ExecuteDeleteAsync();
    }
}
