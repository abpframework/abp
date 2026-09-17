using System.Collections.Generic;

namespace Volo.Abp.AI;

public class AbpAIOptions
{
    public HashSet<string> ConfiguredWorkspaceNames { get; } = new();
}