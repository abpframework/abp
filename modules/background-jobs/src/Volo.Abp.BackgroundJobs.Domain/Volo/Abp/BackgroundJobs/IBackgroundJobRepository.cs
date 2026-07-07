using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.BackgroundJobs;

public interface IBackgroundJobRepository : IBasicRepository<BackgroundJobRecord, Guid>
{
    Task<List<BackgroundJobRecord>> GetWaitingListAsync(
        string? applicationName,
        int maxResultCount,
        CancellationToken cancellationToken = default);

    Task<List<BackgroundJobRecord>> GetWaitingListAsync(
        string? applicationName,
        int maxResultCount,
        BackgroundJobNameFilter? jobNameFilter,
        CancellationToken cancellationToken = default);

    Task<int> DeleteAsync(
        string? applicationName,
        DateTime completedBefore,
        int maxResultCount,
        CancellationToken cancellationToken = default);
}
