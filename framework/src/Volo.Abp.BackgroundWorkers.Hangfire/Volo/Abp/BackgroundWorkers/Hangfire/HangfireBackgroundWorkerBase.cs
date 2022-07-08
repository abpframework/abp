using System;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.BackgroundWorkers.Hangfire;

public abstract class HangfireBackgroundWorkerBase : BackgroundWorkerBase, IHangfireBackgroundWorker
{
    public string RecurringJobId { get; set; }

    public string CronExpression { get; set; }

    public TimeZoneInfo TimeZone { get; set; }

    public string Queue { get; set; }

    public abstract Task DoWorkAsync(CancellationToken cancellationToken = default);

    protected HangfireBackgroundWorkerBase()
    {
        TimeZone = null;
        Queue = "default";
    }
}
