using System;
using System.Collections.Generic;

namespace Volo.Abp.OperationRateLimiting;

public class AbpOperationRateLimitingOptions
{
    public bool IsEnabled { get; set; } = true;

    public TimeSpan LockTimeout { get; set; } = TimeSpan.FromSeconds(5);

    public Dictionary<string, OperationRateLimitingPolicy> Policies { get; } = new();

    public void AddPolicy(string name, Action<OperationRateLimitingPolicyBuilder> configure)
    {
        var builder = new OperationRateLimitingPolicyBuilder(name);
        configure(builder);
        Policies[name] = builder.Build();
    }
}
