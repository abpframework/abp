using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Timing;

namespace Volo.Abp.BackgroundJobs;

public class InMemoryBackgroundJobStore : IBackgroundJobStore, ISingletonDependency
{
    private readonly ConcurrentDictionary<Guid, BackgroundJobInfo> _jobs;

    protected IClock Clock { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InMemoryBackgroundJobStore"/> class.
    /// </summary>
    public InMemoryBackgroundJobStore(IClock clock)
    {
        Clock = clock;
        _jobs = new ConcurrentDictionary<Guid, BackgroundJobInfo>();
    }

    public virtual Task<BackgroundJobInfo> FindAsync(Guid jobId)
    {
        return Task.FromResult(_jobs.GetOrDefault(jobId))!;
    }

    public virtual Task InsertAsync(BackgroundJobInfo jobInfo)
    {
        _jobs[jobInfo.Id] = jobInfo;

        return Task.CompletedTask;
    }

    public virtual Task<List<BackgroundJobInfo>> GetWaitingJobsAsync(string? applicationName, int maxResultCount)
    {
        return GetWaitingJobsAsync(applicationName, maxResultCount, null);
    }

    public virtual Task<List<BackgroundJobInfo>> GetWaitingJobsAsync(
        string? applicationName,
        int maxResultCount,
        BackgroundJobNameFilter? jobNameFilter)
    {
        var filter = jobNameFilter ?? BackgroundJobNameFilter.None;

        var waitingJobs = _jobs.Values
            .Where(t => t.ApplicationName == applicationName)
            .Where(t => !t.IsAbandoned && t.CompletionTime == null && t.NextTryTime <= Clock.Now)
            .Where(t => filter.IsMatch(t.JobName))
            .OrderByDescending(t => t.Priority)
            .ThenBy(t => t.TryCount)
            .ThenBy(t => t.NextTryTime)
            .Take(maxResultCount)
            .ToList();

        return Task.FromResult(waitingJobs);
    }


    public virtual Task DeleteAsync(Guid jobId)
    {
        _jobs.TryRemove(jobId, out _);

        return Task.CompletedTask;
    }

    public virtual Task<int> DeleteAsync(
        string? applicationName,
        DateTime completedBefore,
        int maxResultCount,
        CancellationToken cancellationToken = default)
    {
        var idsToDelete = _jobs.Values
            .Where(t => t.ApplicationName == applicationName)
            .Where(t => t.CompletionTime != null && t.CompletionTime < completedBefore)
            .OrderBy(t => t.CompletionTime)
            .Take(maxResultCount)
            .Select(t => t.Id)
            .ToList();

        foreach (var id in idsToDelete)
        {
            _jobs.TryRemove(id, out _);
        }

        return Task.FromResult(idsToDelete.Count);
    }

    public virtual Task UpdateAsync(BackgroundJobInfo jobInfo)
    {
        if (jobInfo.IsAbandoned)
        {
            return DeleteAsync(jobInfo.Id);
        }

        return Task.CompletedTask;
    }
}
