using Microsoft.Extensions.AI;

namespace Volo.Abp.AgentTools;

/// <summary>A tool offered to the current user: its definition, the effective approval flag and the function to call.</summary>
public class AgentTool
{
    public AgentToolDefinition Definition { get; }

    /// <summary>Effective value from the policy store.</summary>
    public bool RequiresApproval { get; }

    /// <summary>
    /// The function to hand to an <c>IChatClient</c>: wrapped in MEAI's approval marker when <see cref="RequiresApproval"/>,
    /// so a function-invoking client asks a human before calling it.
    /// </summary>
    public AIFunction Function { get; }

    /// <summary>The plain function, never approval-wrapped (e.g. for a host that runs its own approval flow).</summary>
    public AIFunction UnwrappedFunction { get; }

    public AgentTool(AgentToolDefinition definition, AIFunction function, bool requiresApproval)
    {
        Definition = definition;
        UnwrappedFunction = function;
        RequiresApproval = requiresApproval;

        // The only place that touches MEAI's experimental approval types, so their churn stays here.
#pragma warning disable MEAI001 // ApprovalRequiredAIFunction is experimental
        Function = requiresApproval ? new ApprovalRequiredAIFunction(function) : function;
#pragma warning restore MEAI001
    }

    public override string ToString() => Definition.Name;
}
