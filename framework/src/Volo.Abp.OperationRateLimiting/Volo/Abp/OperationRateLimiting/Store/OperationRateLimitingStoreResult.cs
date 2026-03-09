using System;

namespace Volo.Abp.OperationRateLimiting;

public class OperationRateLimitingStoreResult
{
    public bool IsAllowed { get; set; }

    public int CurrentCount { get; set; }

    public int MaxCount { get; set; }

    public TimeSpan? RetryAfter { get; set; }
}
