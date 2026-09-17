using System;

namespace Volo.Abp.BackgroundWorkers;

public class DynamicBackgroundWorkerSchedule
{
    public const int DefaultPeriod = 60000;

    public int? Period { get; set; }

    public string? CronExpression { get; set; }

    public virtual void Validate()
    {
        if (Period.HasValue && Period.Value <= 0)
        {
            throw new ArgumentException(
                $"Period must be greater than 0 when provided. Given value: {Period.Value}.",
                nameof(Period));
        }

        if (Period == null && string.IsNullOrWhiteSpace(CronExpression))
        {
            throw new ArgumentException(
                "At least one of 'Period' or 'CronExpression' must be set.");
        }
    }
}
