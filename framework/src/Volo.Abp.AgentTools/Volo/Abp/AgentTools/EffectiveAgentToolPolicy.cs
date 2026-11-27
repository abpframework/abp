namespace Volo.Abp.AgentTools;

/// <summary>Effective policy of a tool for the current tenant.</summary>
public sealed record EffectiveAgentToolPolicy(bool Enabled, bool RequiresApproval);
