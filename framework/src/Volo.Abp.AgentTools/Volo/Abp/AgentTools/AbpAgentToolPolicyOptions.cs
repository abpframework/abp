using System;
using System.Collections.Generic;

namespace Volo.Abp.AgentTools;

/// <summary>
/// Per tool: is it enabled, does it require human approval. The same for the host and every tenant. Set with Configure in
/// code or in the configuration, which is read once, after that code, and overrides it per field:
/// <code>
/// "AgentTools": {
///   "Tools": {
///     "Product_Delete": { "Enabled": false },
///     "Product_Create": { "RequiresApproval": false }
///   }
/// }
/// </code>
/// A field that is not set keeps the code default of the tool. A tool that requires approval is not offered over MCP.
/// Anything else in the section (an unknown field, a value other than true and false) is an error, never ignored. Settings elsewhere
/// under "AgentTools" (other than "Mcp") that look like a tool policy (a misspelled "Tools", a tool directly under it) have no
/// effect; an error that names their keys is logged at startup and the application starts. A limit: null means "not set": a
/// field set to null in a later configuration source undoes the true or false of an earlier one.
/// </summary>
public class AbpAgentToolPolicyOptions
{
    public const string ConfigurationSection = "AgentTools:Tools";

    /// <summary>By tool name, case-insensitive.</summary>
    public Dictionary<string, AgentToolPolicyConfiguration> Tools { get; } = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>Per field: the configured value, else the code default.</summary>
    public EffectiveAgentToolPolicy GetPolicy(AgentToolDefinition definition)
    {
        var tool = Tools.GetValueOrDefault(definition.Name);
        return new EffectiveAgentToolPolicy(tool?.Enabled ?? true, tool?.RequiresApproval ?? definition.RequiresApprovalByDefault);
    }
}

/// <summary>What is configured for a tool; null = not set.</summary>
public class AgentToolPolicyConfiguration
{
    public bool? Enabled { get; set; }

    public bool? RequiresApproval { get; set; }
}
