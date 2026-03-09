using System;
using System.Collections.Generic;

namespace Volo.Abp.OperationRateLimiting;

public class OperationRateLimitingResult
{
    public bool IsAllowed { get; set; }

    public int RemainingCount { get; set; }

    public int MaxCount { get; set; }

    public int CurrentCount { get; set; }

    public TimeSpan? RetryAfter { get; set; }

    public TimeSpan WindowDuration { get; set; }

    /// <summary>
    /// Detailed results per rule (for composite policies).
    /// </summary>
    public List<OperationRateLimitingRuleResult>? RuleResults { get; set; }
}
