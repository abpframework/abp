using System;

namespace Volo.Abp.BackgroundWorkers.TickerQ;

public class AbpTickerQCronBackgroundWorker
{
    public string Function { get; set; } = null!;

    public string CronExpression { get; set; } = null!;

    public Type WorkerType { get; set; } = null!;
}
