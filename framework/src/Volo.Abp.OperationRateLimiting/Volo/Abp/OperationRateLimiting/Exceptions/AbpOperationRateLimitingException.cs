using System;
using Volo.Abp.ExceptionHandling;

namespace Volo.Abp.OperationRateLimiting;

public class AbpOperationRateLimitingException : BusinessException, IHasHttpStatusCode
{
    public string PolicyName { get; }

    public OperationRateLimitingResult Result { get; }

    public int HttpStatusCode => 429;

    public AbpOperationRateLimitingException(
        string policyName,
        OperationRateLimitingResult result,
        string? errorCode = null)
        : base(code: errorCode ?? AbpOperationRateLimitingErrorCodes.ExceedLimit)
    {
        PolicyName = policyName;
        Result = result;

        WithData("PolicyName", policyName);
        WithData("MaxCount", result.MaxCount);
        WithData("CurrentCount", result.CurrentCount);
        WithData("RemainingCount", result.RemainingCount);
        WithData("RetryAfterSeconds", (int)(result.RetryAfter?.TotalSeconds ?? 0));
        WithData("RetryAfterMinutes", (int)(result.RetryAfter?.TotalMinutes ?? 0));
        WithData("WindowDurationSeconds", (int)result.WindowDuration.TotalSeconds);
    }

    internal void SetRetryAfterFormatted(string formattedRetryAfter)
    {
        WithData("RetryAfter", formattedRetryAfter);
    }

    internal void SetWindowDescriptionFormatted(string formattedWindowDescription)
    {
        WithData("WindowDescription", formattedWindowDescription);
    }
}
