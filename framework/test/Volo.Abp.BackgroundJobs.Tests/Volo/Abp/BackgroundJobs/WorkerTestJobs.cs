using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundJobs;

public class ParallelJobTracker
{
    public ConcurrentBag<string> Executed { get; } = new ConcurrentBag<string>();

    public ConcurrentBag<Guid> ScopeIds { get; } = new ConcurrentBag<Guid>();
}

/// <summary>
/// Scoped service used to verify that each parallel job runs in its own service scope.
/// </summary>
public class ScopeMarker
{
    public Guid Id { get; } = Guid.NewGuid();
}

public class ParallelTestJobArgs
{
    public string Value { get; set; } = default!;
}

public class ParallelTestJob : AsyncBackgroundJob<ParallelTestJobArgs>, ITransientDependency
{
    private readonly ParallelJobTracker _tracker;
    private readonly ScopeMarker _scopeMarker;

    public ParallelTestJob(ParallelJobTracker tracker, ScopeMarker scopeMarker)
    {
        _tracker = tracker;
        _scopeMarker = scopeMarker;
    }

    public override Task ExecuteAsync(ParallelTestJobArgs args)
    {
        _tracker.Executed.Add(args.Value);
        _tracker.ScopeIds.Add(_scopeMarker.Id);
        return Task.CompletedTask;
    }
}

public class WorkerJobAArgs
{
    public string Value { get; set; } = default!;
}

public class WorkerJobA : AsyncBackgroundJob<WorkerJobAArgs>, ITransientDependency
{
    public override Task ExecuteAsync(WorkerJobAArgs args)
    {
        return Task.CompletedTask;
    }
}

public class WorkerJobBArgs
{
    public string Value { get; set; } = default!;
}

public class WorkerJobB : AsyncBackgroundJob<WorkerJobBArgs>, ITransientDependency
{
    public override Task ExecuteAsync(WorkerJobBArgs args)
    {
        return Task.CompletedTask;
    }
}

// Two different args types that resolve to the same job name, to exercise the manager's
// backstop validation (eager AddDedicatedWorker validation compares by type, not resolved name).
[BackgroundJobName("shared-job-name")]
public class SharedNameJobAArgs
{
}

[BackgroundJobName("shared-job-name")]
public class SharedNameJobBArgs
{
}

public class SharedNameJobA : AsyncBackgroundJob<SharedNameJobAArgs>, ITransientDependency
{
    public override Task ExecuteAsync(SharedNameJobAArgs args)
    {
        return Task.CompletedTask;
    }
}

public class SharedNameJobB : AsyncBackgroundJob<SharedNameJobBArgs>, ITransientDependency
{
    public override Task ExecuteAsync(SharedNameJobBArgs args)
    {
        return Task.CompletedTask;
    }
}
