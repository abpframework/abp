using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.OperationRateLimiting;

public class DefaultOperationRateLimitingPolicyProvider : IOperationRateLimitingPolicyProvider, ITransientDependency
{
    protected AbpOperationRateLimitingOptions Options { get; }

    public DefaultOperationRateLimitingPolicyProvider(IOptions<AbpOperationRateLimitingOptions> options)
    {
        Options = options.Value;
    }

    public virtual Task<OperationRateLimitingPolicy> GetAsync(string policyName)
    {
        if (!Options.Policies.TryGetValue(policyName, out var policy))
        {
            throw new AbpException(
                $"Operation rate limit policy '{policyName}' was not found. " +
                $"Make sure to configure it using AbpOperationRateLimitingOptions.AddPolicy().");
        }

        return Task.FromResult(policy);
    }

    public virtual Task<List<OperationRateLimitingPolicy>> GetListAsync()
    {
        return Task.FromResult(Options.Policies.Values.ToList());
    }
}
