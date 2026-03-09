using System.Collections.Generic;
using System.Threading.Tasks;

namespace Volo.Abp.OperationRateLimiting;

public interface IOperationRateLimitingPolicyProvider
{
    Task<OperationRateLimitingPolicy> GetAsync(string policyName);

    Task<List<OperationRateLimitingPolicy>> GetListAsync();
}
