using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.AgentTools;

public interface IAgentToolProvider
{
    /// <summary>
    /// The current user's permitted, enabled tools. Tools that require approval are marked
    /// (<see cref="AgentTool.RequiresApproval"/>), or left out when the context does not support approval.
    /// </summary>
    Task<IReadOnlyList<AgentTool>> GetToolsAsync(AgentToolContext context, CancellationToken cancellationToken = default);
}
