﻿using System;
using System.Collections.Generic;
using System.Linq;
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
        dbContext.OutgoingEvents.Add(
            new OutgoingEventRecord(outgoingEvent)
        );
    }

    [UnitOfWork]
    public virtual async Task<List<OutgoingEventInfo>> GetWaitingEventsAsync(int maxCount, CancellationToken cancellationToken = default)
    {
        var dbContext = (IHasEventOutbox)await DbContextProvider.GetDbContextAsync();

        var outgoingEventRecords = await dbContext
            .OutgoingEvents
            .AsNoTracking()
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
        var outgoingEvent = await dbContext.OutgoingEvents.FindAsync(id);
        if (outgoingEvent != null)
        {
            dbContext.Remove(outgoingEvent);
        }
    }

    [UnitOfWork]
    public virtual async Task DeleteManyAsync(IEnumerable<Guid> ids)
    {
        var dbContext = (IHasEventOutbox)await DbContextProvider.GetDbContextAsync();
        var outgoingEvents = await dbContext.OutgoingEvents.Where(x => ids.Contains(x.Id)).ToListAsync();
        if (outgoingEvents.Any())
        {
            dbContext.RemoveRange(outgoingEvents);
        }
    }
}
