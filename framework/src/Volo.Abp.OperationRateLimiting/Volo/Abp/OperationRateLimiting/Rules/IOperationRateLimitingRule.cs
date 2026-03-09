using System.Threading.Tasks;

namespace Volo.Abp.OperationRateLimiting;

public interface IOperationRateLimitingRule
{
    Task<OperationRateLimitingRuleResult> AcquireAsync(OperationRateLimitingContext context);

    Task<OperationRateLimitingRuleResult> CheckAsync(OperationRateLimitingContext context);

    Task ResetAsync(OperationRateLimitingContext context);
}
