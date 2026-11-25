using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.AgentTools;

public interface IAgentToolPolicyStore
{
    /// <summary>The policy of the tool for the current tenant (ICurrentTenant.Id; null = host).</summary>
    Task<EffectiveAgentToolPolicy> GetAsync(AgentToolDefinition definition, CancellationToken cancellationToken = default);

    /// <summary>The policies of the given tools for the current tenant, keyed by tool name.</summary>
    Task<IReadOnlyDictionary<string, EffectiveAgentToolPolicy>> GetListAsync(
        IReadOnlyList<AgentToolDefinition> definitions,
        CancellationToken cancellationToken = default);
}
