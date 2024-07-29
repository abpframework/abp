using System;
using System.Threading;
using System.Threading.Tasks;
using Hangfire.States;

namespace Volo.Abp.BackgroundWorkers.Hangfire;

public abstract class HangfireBackgroundWorkerBase : BackgroundWorkerBase, IHangfireBackgroundWorker
{
    public string? RecurringJobId { get; set; }

    public string CronExpression { get; set; } = default!;

    public TimeZoneInfo? TimeZone { get; set; } = TimeZoneInfo.Utc;

    public string Queue { get; set; } = EnqueuedState.DefaultQueue;

    public abstract Task DoWorkAsync(CancellationToken cancellationToken = default);
}
