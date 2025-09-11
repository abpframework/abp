using System;

namespace Volo.Abp.Internal.Telemetry.Activity.Storage;

internal class FailedActivityInfo
{
    public DateTimeOffset FirstFailTime { get; set; }
    public DateTimeOffset LastFailTime { get; set; }
    public int RetryCount { get; set; }

    public bool IsExpired()
    {
        var now = DateTimeOffset.UtcNow;

        return RetryCount >= TelemetryPeriod.MaxActivityRetryCount ||
               now - FirstFailTime > TelemetryPeriod.MaxFailedActivityAge;
    }
}