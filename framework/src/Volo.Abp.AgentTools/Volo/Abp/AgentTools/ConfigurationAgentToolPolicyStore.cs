using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Volo.Abp.AgentTools;

/// <summary>Default store: <see cref="AbpAgentToolPolicyOptions"/>, the same for the host and every tenant.</summary>
public class ConfigurationAgentToolPolicyStore : IAgentToolPolicyStore
{
    protected IOptions<AbpAgentToolPolicyOptions> Options { get; }

    public ConfigurationAgentToolPolicyStore(IOptions<AbpAgentToolPolicyOptions> options)
    {
        Options = options;
    }

    public virtual Task<EffectiveAgentToolPolicy> GetAsync(AgentToolDefinition definition, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Options.Value.GetPolicy(definition));
    }

    public virtual Task<IReadOnlyDictionary<string, EffectiveAgentToolPolicy>> GetListAsync(
        IReadOnlyList<AgentToolDefinition> definitions,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyDictionary<string, EffectiveAgentToolPolicy>>(
            definitions.ToDictionary(d => d.Name, Options.Value.GetPolicy));
    }
}
