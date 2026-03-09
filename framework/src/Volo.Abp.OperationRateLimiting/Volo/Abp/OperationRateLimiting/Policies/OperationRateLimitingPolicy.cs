using System;
using System.Collections.Generic;

namespace Volo.Abp.OperationRateLimiting;

public class OperationRateLimitingPolicy
{
    public string Name { get; set; } = default!;

    public string? ErrorCode { get; set; }

    public List<OperationRateLimitingRuleDefinition> Rules { get; set; } = new();

    public List<Type> CustomRuleTypes { get; set; } = new();
}
