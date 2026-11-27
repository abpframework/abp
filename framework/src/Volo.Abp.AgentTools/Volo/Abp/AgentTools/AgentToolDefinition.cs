using System;
using System.Collections.Generic;
using System.Reflection;

namespace Volo.Abp.AgentTools;

/// <summary>One HTTP API action exposed as an agent tool. Invocation always goes through <see cref="ServiceInterface"/>.</summary>
public sealed class AgentToolDefinition
{
    public required string Name { get; init; }

    public required string Description { get; init; }

    public required AgentToolKind Kind { get; init; }

    public required bool ReadOnly { get; init; }

    public required bool Destructive { get; init; }

    public required bool Idempotent { get; init; }

    /// <summary>Code default for approval (verb plus <see cref="AgentToolAttribute"/>); the policy store may override it.</summary>
    public required bool RequiresApprovalByDefault { get; init; }

    /// <summary>The service interface resolved from DI for a call; never the implementation class or the controller.</summary>
    public required Type ServiceInterface { get; init; }

    /// <summary>The method on the interface (it may be declared on a base interface of <see cref="ServiceInterface"/>).</summary>
    public required MethodInfo InterfaceMethod { get; init; }

    /// <summary>
    /// The class registered for <see cref="ServiceInterface"/> in DI (unproxied), resolved once when the catalogue is built.
    /// Used for its attributes ([DisableAgentTool], [AgentTool], [Description]) and for the permission pre-check; never invoked directly.
    /// </summary>
    public required Type ImplementationType { get; init; }

    /// <summary><see cref="InterfaceMethod"/> mapped onto <see cref="ImplementationType"/>: where ABP's interceptors read [Authorize], [RequiresFeature] etc.</summary>
    public required MethodInfo ImplementationMethod { get; init; }

    /// <summary>
    /// The HTTP action method of a hand-written controller (which is not the implementation), or null for conventional
    /// controllers. Its [Authorize] / [RequiresFeature] are checked on every call too, so in-process calls cannot bypass them.
    /// </summary>
    public MethodInfo? ControllerActionMethod { get; init; }

    /// <summary>The hand-written controller type (set with <see cref="ControllerActionMethod"/>); its class-level attributes count even when the action is declared on a base controller.</summary>
    public Type? ControllerType { get; init; }

    public required string HttpMethod { get; init; }

    /// <summary>Relative URL of the HTTP endpoint, e.g. "api/app/book/{id}".</summary>
    public required string Url { get; init; }

    public required string RemoteServiceName { get; init; }

    public required string ModuleRootPath { get; init; }

    public required string ControllerName { get; init; }

    /// <summary>Action (method) name as in the API description model, e.g. "GetListAsync".</summary>
    public required string ActionName { get; init; }

    /// <summary>Parameters of <see cref="InterfaceMethod"/> in order, without the CancellationToken.</summary>
    public required IReadOnlyList<AgentToolParameter> Parameters { get; init; }

    /// <summary><see cref="AbpAgentToolOptions.RequireNonNullableValueTypes"/> when the catalogue was built; read by <see cref="AgentToolJsonSchema"/>.</summary>
    public bool RequireNonNullableValueTypes { get; init; } = true;

    public override string ToString() => Name;
}

/// <summary>A parameter of <see cref="AgentToolDefinition.InterfaceMethod"/>; IsRequired comes from the API description model.</summary>
public sealed record AgentToolParameter(string Name, Type Type, bool IsRequired, string? Description);
