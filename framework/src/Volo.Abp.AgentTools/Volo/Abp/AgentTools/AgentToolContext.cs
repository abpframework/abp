namespace Volo.Abp.AgentTools;

/// <summary>Who asks for tools or calls one.</summary>
public class AgentToolContext
{
    /// <summary>
    /// True when the caller can ask a human before running a tool that requires approval (e.g. a chat UI).
    /// False hides and refuses such tools (e.g. MCP without elicitation).
    /// </summary>
    public bool SupportsApproval { get; init; }

    /// <summary>OAuth client id or similar, for the audit comment and the call record.</summary>
    public string? ClientId { get; init; }

    public string? ClientName { get; init; }

    /// <summary>
    /// Set by the function of a listed tool: the approval flag the tool was listed with. When the policy now requires
    /// approval but the tool was listed without it (a stale tool list), the call is refused, even with
    /// <see cref="SupportsApproval"/>. Null for calls by name.
    /// </summary>
    public bool? ListedRequiresApproval { get; init; }
}
