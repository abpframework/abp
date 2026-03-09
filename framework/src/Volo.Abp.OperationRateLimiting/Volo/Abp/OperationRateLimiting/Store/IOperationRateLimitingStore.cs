using System;
using System.Threading.Tasks;

namespace Volo.Abp.OperationRateLimiting;

public interface IOperationRateLimitingStore
{
    Task<OperationRateLimitingStoreResult> IncrementAsync(string key, TimeSpan duration, int maxCount);

    Task<OperationRateLimitingStoreResult> GetAsync(string key, TimeSpan duration, int maxCount);

    Task ResetAsync(string key);
}
