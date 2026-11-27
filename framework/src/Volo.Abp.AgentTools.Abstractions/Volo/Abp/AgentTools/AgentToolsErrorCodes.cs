namespace Volo.Abp.AgentTools;

/// <summary>Error codes of tool calls the invoker refuses before calling the service (localized in AbpAgentToolsResource).</summary>
public static class AgentToolsErrorCodes
{
    /// <summary>No tool with that name.</summary>
    public const string ToolNotFound = "AgentTools:ToolNotFound";

    public const string ToolDisabled = "AgentTools:ToolDisabled";

    public const string ApprovalNotSupported = "AgentTools:ApprovalNotSupported";

    public const string ApprovalPolicyChanged = "AgentTools:ApprovalPolicyChanged";

    /// <summary>The AgentTools.Enable feature is off for the current tenant.</summary>
    public const string FeatureDisabled = "AgentTools:FeatureDisabled";
}
