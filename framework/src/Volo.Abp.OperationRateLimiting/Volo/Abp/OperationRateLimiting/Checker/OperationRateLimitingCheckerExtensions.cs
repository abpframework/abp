using System.Threading.Tasks;

namespace Volo.Abp.OperationRateLimiting;

public static class OperationRateLimitingCheckerExtensions
{
    public static Task CheckAsync(
        this IOperationRateLimitingChecker checker,
        string policyName,
        string parameter)
    {
        return checker.CheckAsync(policyName, new OperationRateLimitingContext { Parameter = parameter });
    }

    public static Task<bool> IsAllowedAsync(
        this IOperationRateLimitingChecker checker,
        string policyName,
        string parameter)
    {
        return checker.IsAllowedAsync(policyName, new OperationRateLimitingContext { Parameter = parameter });
    }

    public static Task<OperationRateLimitingResult> GetStatusAsync(
        this IOperationRateLimitingChecker checker,
        string policyName,
        string parameter)
    {
        return checker.GetStatusAsync(policyName, new OperationRateLimitingContext { Parameter = parameter });
    }

    public static Task ResetAsync(
        this IOperationRateLimitingChecker checker,
        string policyName,
        string parameter)
    {
        return checker.ResetAsync(policyName, new OperationRateLimitingContext { Parameter = parameter });
    }
}
