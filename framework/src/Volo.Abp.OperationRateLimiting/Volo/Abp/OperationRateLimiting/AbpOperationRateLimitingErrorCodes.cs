namespace Volo.Abp.OperationRateLimiting;

public static class AbpOperationRateLimitingErrorCodes
{
    /// <summary>
    /// Default error code for rate limit exceeded (with a retry-after window).
    /// </summary>
    public const string ExceedLimit = "Volo.Abp.OperationRateLimiting:010001";

    /// <summary>
    /// Error code for ban policy (maxCount: 0) where requests are permanently denied.
    /// </summary>
    public const string ExceedLimitPermanently = "Volo.Abp.OperationRateLimiting:010002";
}
