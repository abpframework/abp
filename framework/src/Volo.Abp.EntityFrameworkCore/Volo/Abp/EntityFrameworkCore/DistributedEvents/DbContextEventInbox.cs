using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Timing;
using Volo.Abp.Uow;

namespace Volo.Abp.EntityFrameworkCore.DistributedEvents;

public class DbContextEventInbox<TDbContext> : IDbContextEventInbox<TDbContext>
    where TDbContext : IHasEventInbox
{
    protected IDbContextProvider<TDbContext> DbContextProvider { get; }
    protected AbpEventBusBoxesOptions EventBusBoxesOptions { get; }
    protected IClock Clock { get; }

    public DbContextEventInbox(
        IDbContextProvider<TDbContext> dbContextProvider,
        IClock clock,
       IOptions<AbpEventBusBoxesOptions> eventBusBoxesOptions)
    {
        DbContextProvider = dbContextProvider;
        Clock = clock;
        EventBusBoxesOptions = eventBusBoxesOptions.Value;
    }

    [UnitOfWork]
    public virtual async Task EnqueueAsync(IncomingEventInfo incomingEvent)
    {
        var dbContext = await DbContextProvider.GetDbContextAsync();
        dbContext.IncomingEvents.Add(new IncomingEventRecord(incomingEvent));
    }

    [UnitOfWork]
    public virtual async Task<List<IncomingEventInfo>> GetWaitingEventsAsync(int maxCount, CancellationToken cancellationToken = default)
    {
        var dbContext = await DbContextProvider.GetDbContextAsync();

        var outgoingEventRecords = await dbContext
            .IncomingEvents
            .AsNoTracking()
            .Where(x => !x.Processed)
            .OrderBy(x => x.CreationTime)
            .Take(maxCount)
            .ToListAsync(cancellationToken: cancellationToken);

        return outgoingEventRecords
            .Select(x => x.ToIncomingEventInfo())
            .ToList();
    }

    [UnitOfWork]
    public virtual async Task MarkAsProcessedAsync(Guid id)
    {
        var dbContext = await DbContextProvider.GetDbContextAsync();
        await dbContext.IncomingEvents.Where(x => x.Id == id).ExecuteUpdateAsync(x =>
            x.SetProperty(p => p.Processed, _ => true).SetProperty(p => p.ProcessedTime, _ => Clock.Now));
    }

    [UnitOfWork]
    public virtual async Task<bool> ExistsByMessageIdAsync(string messageId)
    {
        var dbContext = await DbContextProvider.GetDbContextAsync();
        return await dbContext.IncomingEvents.AnyAsync(x => x.MessageId == messageId);
    }

    [UnitOfWork]
    public virtual async Task DeleteOldEventsAsync()
    {
        var dbContext = await DbContextProvider.GetDbContextAsync();
        var timeToKeepEvents = Clock.Now - EventBusBoxesOptions.WaitTimeToDeleteProcessedInboxEvents;
        await dbContext.IncomingEvents
            .Where(x => x.Processed && x.CreationTime < timeToKeepEvents)
            .ExecuteDeleteAsync();
    }
}
