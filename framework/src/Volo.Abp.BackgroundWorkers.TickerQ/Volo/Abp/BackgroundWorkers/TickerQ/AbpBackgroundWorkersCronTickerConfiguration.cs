using TickerQ.Utilities.Enums;

namespace Volo.Abp.BackgroundWorkers.TickerQ;

public class AbpBackgroundWorkersCronTickerConfiguration
{
    public int? Retries { get; set; }

    public int[]? RetryIntervals { get; set; }

    public TickerTaskPriority? Priority { get; set; }
}
