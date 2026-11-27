using System;

namespace Volo.Abp.AgentTools;

/// <summary>One tool call, for audit comments and activity views.</summary>
public class AgentToolCallRecord
{
    public required string ToolName { get; init; }

    /// <summary>Arguments as JSON with password-like values redacted.</summary>
    public required string Arguments { get; init; }

    public bool Succeeded { get; init; }

    /// <summary>Full type name of the exception when the call failed (OperationCanceledException when it was cancelled).</summary>
    public string? ErrorType { get; init; }

    /// <summary>ABP error code (IHasErrorCode) when present.</summary>
    public string? ErrorCode { get; init; }

    public string? ErrorMessage { get; init; }

    public DateTime StartTime { get; init; }

    public TimeSpan Duration { get; init; }

    public Guid? UserId { get; init; }

    public string? UserName { get; init; }

    public Guid? TenantId { get; init; }

    public string? ClientId { get; init; }

    /// <summary><see cref="AgentToolContext.ClientName"/>, e.g. "MCP external agent: claude-code 1.0".</summary>
    public string? ClientName { get; init; }
}
