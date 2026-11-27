using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.AgentTools;

public interface IAgentToolInvoker
{
    /// <summary>
    /// Calls a tool by name through its service interface in its own unit of work. Returns the result as JSON,
    /// or "ERROR &lt;ExceptionType&gt;: &lt;message&gt;" for failures and for unknown, disabled or
    /// approval-required-without-approval-support tools. Does not throw for tool failures.
    /// Accepts any argument dictionary, including MEAI's AIFunctionArguments.
    /// </summary>
    Task<string> InvokeAsync(
        string toolName,
        IDictionary<string, object?> arguments,
        AgentToolContext context,
        CancellationToken cancellationToken = default);
}
