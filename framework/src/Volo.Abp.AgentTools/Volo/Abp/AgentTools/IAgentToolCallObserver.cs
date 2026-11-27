using System.Threading.Tasks;

namespace Volo.Abp.AgentTools;

/// <summary>
/// Receives one record per tool call. Every registered observer is called;
/// a failing observer is logged and never breaks the call.
/// </summary>
public interface IAgentToolCallObserver
{
    Task OnCallAsync(AgentToolCallRecord record);
}
