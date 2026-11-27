using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AgentTools;

/// <summary>
/// What <see cref="Set"/> stored for the current tenant, else for the host (null), else the code default; a null field keeps
/// the code default. Register it as a singleton <see cref="IAgentToolPolicyStore"/>.
/// </summary>
public class InMemoryAgentToolPolicyStore : IAgentToolPolicyStore
{
    private readonly ConcurrentDictionary<(string ToolName, Guid? TenantId), (bool? Enabled, bool? RequiresApproval)> _policies = new();

    private readonly ICurrentTenant _currentTenant;

    public InMemoryAgentToolPolicyStore(ICurrentTenant currentTenant)
    {
        _currentTenant = currentTenant;
    }

    public void Set(string toolName, Guid? tenantId, bool? enabled, bool? requiresApproval)
    {
        _policies[(toolName, tenantId)] = (enabled, requiresApproval);
    }

    public Task<EffectiveAgentToolPolicy> GetAsync(AgentToolDefinition definition, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Get(definition));
    }

    public Task<IReadOnlyDictionary<string, EffectiveAgentToolPolicy>> GetListAsync(
        IReadOnlyList<AgentToolDefinition> definitions,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyDictionary<string, EffectiveAgentToolPolicy>>(definitions.ToDictionary(d => d.Name, Get));
    }

    private EffectiveAgentToolPolicy Get(AgentToolDefinition definition)
    {
        if (!_policies.TryGetValue((definition.Name, _currentTenant.Id), out var policy))
        {
            _policies.TryGetValue((definition.Name, null), out policy);
        }

        return new EffectiveAgentToolPolicy(policy.Enabled ?? true, policy.RequiresApproval ?? definition.RequiresApprovalByDefault);
    }
}
