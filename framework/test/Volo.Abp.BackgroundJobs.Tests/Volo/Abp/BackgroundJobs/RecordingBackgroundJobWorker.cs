using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundJobs;

/// <summary>
/// Records every <see cref="IBackgroundJobWorker.StartAsync"/> call so multi-worker registration
/// (resolved from DI by <see cref="BackgroundJobWorkerManager"/>) can be asserted. Does not start any timer.
/// </summary>
public class WorkerStartRecorder
{
    public List<WorkerStartRecord> Records { get; } = new List<WorkerStartRecord>();
}

public class WorkerStartRecord
{
    public string? DistributedLockName { get; set; }

    public BackgroundJobNameFilter? JobNameFilter { get; set; }
}

public class RecordingBackgroundJobWorker : IBackgroundJobWorker, ITransientDependency
{
    private readonly WorkerStartRecorder _recorder;

    public RecordingBackgroundJobWorker(WorkerStartRecorder recorder)
    {
        _recorder = recorder;
    }

    public Task StartAsync(
        string? distributedLockName = null,
        BackgroundJobNameFilter? jobNameFilter = null,
        CancellationToken cancellationToken = default)
    {
        _recorder.Records.Add(new WorkerStartRecord
        {
            DistributedLockName = distributedLockName,
            JobNameFilter = jobNameFilter
        });

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
