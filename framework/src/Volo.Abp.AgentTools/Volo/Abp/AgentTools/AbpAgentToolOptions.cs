using System;
using System.Collections.Generic;

namespace Volo.Abp.AgentTools;

/// <summary>
/// Which HTTP API actions become agent tools. Exposure is opt-in: with nothing included the catalogue is empty.
/// An action is included when its remote service name, its module root path or its service interface matches;
/// <see cref="ExcludedServiceTypes"/> wins over every include.
/// </summary>
public class AbpAgentToolOptions
{
    /// <summary>Remote service names (<c>[RemoteService(Name = ...)]</c> / conventional controller setting), case-insensitive.</summary>
    public HashSet<string> IncludedRemoteServiceNames { get; } = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>Module root paths of the API description model (e.g. "app", "identity"), case-insensitive.</summary>
    public HashSet<string> IncludedRootPaths { get; } = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>Predicates over the service interface (the controller type when it has none).</summary>
    public List<Func<Type, bool>> IncludedServiceTypes { get; } = new();

    /// <summary>Predicates over the service interface (the controller type when it has none); exclusion wins.</summary>
    public List<Func<Type, bool>> ExcludedServiceTypes { get; } = new();

    /// <summary>Upper bound for the number of tools; more included tools fail at startup.</summary>
    public int MaxTools { get; set; } = 150;

    /// <summary>
    /// Tool input schemas list a settable non-nullable value-type property (int, bool, Guid, DateTime, an enum, a struct) of a
    /// DTO as required unless it has a default: an initializer or [DefaultValue]. Default true. [Required], [JsonRequired]
    /// and the C# <c>required</c> modifier always make a property required; a constructor parameter without a default does
    /// too, except on ABP's paging and sorting members (SkipCount, MaxResultCount, Sorting), which are optional unless
    /// explicitly required.
    /// </summary>
    public bool RequireNonNullableValueTypes { get; set; } = true;

    /// <summary>
    /// A tool whose input schema fails <see cref="AgentToolSchemaValidator"/> is left out of the catalogue with an error log
    /// (one invalid schema makes providers reject every request that carries it). True: startup fails instead. Default false.
    /// </summary>
    public bool FailOnInvalidSchema { get; set; }

    /// <summary>
    /// Name parts whose argument values are redacted in logs and call records, in addition to the built-in list of
    /// <see cref="AgentToolArgumentRedactor"/>. Matched like it: case-insensitive, '_' and '-' ignored, substring.
    /// </summary>
    public List<string> AdditionalSensitiveArgumentNames { get; } = new();

    public bool HasAnyInclusion =>
        IncludedRemoteServiceNames.Count > 0 || IncludedRootPaths.Count > 0 || IncludedServiceTypes.Count > 0;
}
