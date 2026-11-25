using System;

namespace Volo.Abp.AgentTools;

/// <summary>
/// Never expose this method (or every method of this class or interface) as an agent tool,
/// whatever <c>AbpAgentToolOptions</c> includes.
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface)]
public class DisableAgentToolAttribute : Attribute
{
}
