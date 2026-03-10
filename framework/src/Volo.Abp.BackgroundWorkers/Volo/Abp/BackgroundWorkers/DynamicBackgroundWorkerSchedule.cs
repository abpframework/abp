namespace Volo.Abp.BackgroundWorkers;

public class DynamicBackgroundWorkerSchedule
{
    public const int DefaultPeriod = 60000;

    public int? Period { get; set; }

    public string? CronExpression { get; set; }
}
