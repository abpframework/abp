using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.AgentTools.Sample.Diagnostics;

// Remote service "DiagnosticsSample" (root path "diagnostics"), own namespace so the other samples' tool lists stay as they are.

public interface IDiagnosticsSampleAppService : IApplicationService
{
    /// <summary>Tool DiagnosticsSample_CreateWithInvalidPattern: the [RegularExpression] pattern is not a valid regex.</summary>
    Task CreateWithInvalidPatternAsync(InvalidPatternInput input);

    /// <summary>Tool DiagnosticsSample_UpdateFull (PUT): full-replace input.</summary>
    Task UpdateFullAsync(Guid id, FullReplaceInput input);

    /// <summary>Tool DiagnosticsSample_UpdatePatch (PUT): patch-style input.</summary>
    Task UpdatePatchAsync(Guid id, PatchInput input);
}

public class DiagnosticsSampleAppService : ApplicationService, IDiagnosticsSampleAppService
{
    public const string RemoteServiceName = "DiagnosticsSample";
    public const string RootPath = "diagnostics";
    public const string CreateWithInvalidPattern = "DiagnosticsSample_CreateWithInvalidPattern";
    public const string UpdateFull = "DiagnosticsSample_UpdateFull";
    public const string UpdatePatch = "DiagnosticsSample_UpdatePatch";

    public Task CreateWithInvalidPatternAsync(InvalidPatternInput input) => Task.CompletedTask;

    public Task UpdateFullAsync(Guid id, FullReplaceInput input) => Task.CompletedTask;

    public Task UpdatePatchAsync(Guid id, PatchInput input) => Task.CompletedTask;
}

public class InvalidPatternInput
{
    [RegularExpression("[a-z")]
    public string? Code { get; set; }
}

/// <summary>
/// Every property is required (or the id, the concurrency stamp or the extra properties), so an update through it replaces
/// the whole entity. Shaped like ABP's usual update DTO: extensible, with a nullable stamp.
/// </summary>
public class FullReplaceInput : ExtensibleObject
{
    public Guid? Id { get; set; }

    public string? ConcurrencyStamp { get; set; }

    [Required]
    public string? Name { get; set; }

    public int Pages { get; set; }

    public required string Author { get; set; }
}

/// <summary>Null means "leave unchanged".</summary>
public class PatchInput
{
    public string? Name { get; set; }

    public int? Pages { get; set; }
}
