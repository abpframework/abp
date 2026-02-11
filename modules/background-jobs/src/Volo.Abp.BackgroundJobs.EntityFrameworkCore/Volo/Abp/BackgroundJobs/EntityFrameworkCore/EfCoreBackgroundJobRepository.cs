using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
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

    public virtual async Task<List<BackgroundJobRecord>> GetWaitingListAsync([CanBeNull] string applicationName, int maxResultCount, CancellationToken cancellationToken = default)
    {
        return await (await GetWaitingListQueryAsync(applicationName, maxResultCount)).ToListAsync(GetCancellationToken(cancellationToken));
    }

    protected virtual async Task<IQueryable<BackgroundJobRecord>> GetWaitingListQueryAsync([CanBeNull] string applicationName, int maxResultCount)
    {
        var now = Clock.Now;
        return (await GetDbSetAsync())
            .Where(t => t.ApplicationName == applicationName)
            .Where(t => !t.IsAbandoned && t.NextTryTime <= now)
            .OrderByDescending(t => t.Priority)
            .ThenBy(t => t.TryCount)
            .ThenBy(t => t.NextTryTime)
            .Take(maxResultCount);
    }
}
