using System.Collections.Generic;

namespace Volo.Abp.AgentTools;

/// <summary>All agent tools of the application, built once from the API description model.</summary>
public interface IAgentToolCatalog
{
    IReadOnlyList<AgentToolDefinition> Tools { get; }

    /// <summary>Included actions that are not tools, with the reason (also logged when the catalogue is built).</summary>
    IReadOnlyList<SkippedAgentToolAction> Skipped { get; }

    AgentToolDefinition? Find(string name);
}

public sealed record SkippedAgentToolAction(
    string RemoteServiceName,
    string ModuleRootPath,
    string ControllerName,
    string ControllerType,
    string ActionName,
    string Reason);
