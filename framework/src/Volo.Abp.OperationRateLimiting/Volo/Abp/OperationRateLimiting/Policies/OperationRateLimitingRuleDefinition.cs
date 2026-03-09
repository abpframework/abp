using System;
using System.Threading.Tasks;

namespace Volo.Abp.OperationRateLimiting;

public class OperationRateLimitingRuleDefinition
{
    public TimeSpan Duration { get; set; }

    public int MaxCount { get; set; }

    public OperationRateLimitingPartitionType PartitionType { get; set; }

    public Func<OperationRateLimitingContext, Task<string>>? CustomPartitionKeyResolver { get; set; }

    public bool IsMultiTenant { get; set; }
}
