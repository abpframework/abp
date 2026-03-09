using System.Threading.Tasks;

namespace Volo.Abp.OperationRateLimiting;

public interface IOperationRateLimitingChecker
{
    Task CheckAsync(string policyName, OperationRateLimitingContext? context = null);

    Task<bool> IsAllowedAsync(string policyName, OperationRateLimitingContext? context = null);

    Task<OperationRateLimitingResult> GetStatusAsync(string policyName, OperationRateLimitingContext? context = null);

    Task ResetAsync(string policyName, OperationRateLimitingContext? context = null);
}
