using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Timing;

namespace Volo.Abp.BackgroundJobs.EntityFrameworkCore;

public class EfCoreBackgroundJobRepository : EfCoreRepository<IBackgroundJobsDbContext, BackgroundJobRecord, Guid>, IBackgroundJobRepository
{
    protected IClock Clock { get; }

    public EfCoreBackgroundJobRepository(
        IDbContextProvider<IBackgroundJobsDbContext> dbContextProvider,
        IClock clock)
        : base(dbContextProvider)
    {
        Clock = clock;
    }

    public virtual Task<List<BackgroundJobRecord>> GetWaitingListAsync(
        string? applicationName,
        int maxResultCount,
        CancellationToken cancellationToken = default)
    {
        return GetWaitingListAsync(applicationName, maxResultCount, null, cancellationToken);
    }

    public virtual async Task<List<BackgroundJobRecord>> GetWaitingListAsync(
        string? applicationName,
        int maxResultCount,
        BackgroundJobNameFilter? jobNameFilter,
        CancellationToken cancellationToken = default)
    {
        return await (await GetWaitingListQueryAsync(applicationName, maxResultCount, jobNameFilter))
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    protected virtual async Task<IQueryable<BackgroundJobRecord>> GetWaitingListQueryAsync(
        string? applicationName,
        int maxResultCount,
        BackgroundJobNameFilter? jobNameFilter = null)
    {
        var now = Clock.Now;
        var filter = jobNameFilter ?? BackgroundJobNameFilter.None;
        var jobNames = filter.JobNames.ToList();

        var query = (await GetDbSetAsync())
            .Where(t => t.ApplicationName == applicationName)
            .Where(t => !t.IsAbandoned && t.CompletionTime == null && t.NextTryTime <= now);

        if (filter.Mode == BackgroundJobNameFilterMode.Include)
        {
            query = query.Where(t => jobNames.Contains(t.JobName));
        }
        else if (filter.Mode == BackgroundJobNameFilterMode.Exclude)
        {
            query = query.Where(t => !jobNames.Contains(t.JobName));
        }

        return query
            .OrderByDescending(t => t.Priority)
            .ThenBy(t => t.TryCount)
            .ThenBy(t => t.NextTryTime)
            .Take(maxResultCount);
    }

    public virtual async Task<int> DeleteAsync(string? applicationName, DateTime completedBefore, int maxResultCount, CancellationToken cancellationToken = default)
    {
        var token = GetCancellationToken(cancellationToken);
        var dbSet = await GetDbSetAsync();

        var ids = await dbSet
            .Where(t => t.ApplicationName == applicationName)
            .Where(t => t.CompletionTime != null && t.CompletionTime < completedBefore)
            .OrderBy(t => t.CompletionTime)
            .Select(t => t.Id)
            .Take(maxResultCount)
            .ToListAsync(token);

        if (ids.Count == 0)
        {
            return 0;
        }

        return await dbSet.Where(t => ids.Contains(t.Id)).ExecuteDeleteAsync(token);
    }
}
