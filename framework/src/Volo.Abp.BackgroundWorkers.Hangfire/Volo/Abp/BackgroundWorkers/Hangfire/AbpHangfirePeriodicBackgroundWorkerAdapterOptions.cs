using System;

namespace Volo.Abp.BackgroundWorkers.Hangfire;

public class AbpHangfirePeriodicBackgroundWorkerAdapterOptions
{
    public TimeZoneInfo TimeZone { get; set; } = TimeZoneInfo.Utc;

    public string Queue { get; set; } = default!;
}
