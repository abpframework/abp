using System;
using TickerQ.Utilities.Enums;

namespace Volo.Abp.BackgroundJobs.TickerQ;

public class AbpBackgroundJobsTimeTickerConfiguration
{
    public int? Retries { get; set; }

    public int[]? RetryIntervals { get; set; }

    public TickerTaskPriority? Priority { get; set; }

    public Guid? BatchParent { get; set; }

    public BatchRunCondition? BatchRunCondition { get; set; }
}
