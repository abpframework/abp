using System;

namespace Volo.Abp.AgentTools;

/// <summary>
/// Overrides what the catalogue derives for an agent tool (name, description, approval, annotations).
/// Put it on the application service interface method or its implementation; values set on the
/// implementation win per property. Properties that are not set keep the derived value.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class AgentToolAttribute : Attribute
{
    // Attribute named arguments cannot be bool?, so bool properties sit over nullable fields to keep "not set".
    private bool? _requiresApproval;
    private bool? _readOnly;
    private bool? _destructive;
    private bool? _idempotent;

    /// <summary>Tool name; only A-Z, a-z, 0-9, '_' and '-', at most 64 characters.</summary>
    public string? Name { get; set; }

    /// <summary>The description the model sees.</summary>
    public string? Description { get; set; }

    public bool RequiresApproval { get => _requiresApproval ?? false; set => _requiresApproval = value; }

    public bool ReadOnly { get => _readOnly ?? false; set => _readOnly = value; }

    public bool Destructive { get => _destructive ?? false; set => _destructive = value; }

    public bool Idempotent { get => _idempotent ?? false; set => _idempotent = value; }

    /// <summary>The value of <see cref="RequiresApproval"/>, or null when it was not set.</summary>
    public bool? GetRequiresApproval() => _requiresApproval;

    /// <summary>The value of <see cref="ReadOnly"/>, or null when it was not set.</summary>
    public bool? GetReadOnly() => _readOnly;

    /// <summary>The value of <see cref="Destructive"/>, or null when it was not set.</summary>
    public bool? GetDestructive() => _destructive;

    /// <summary>The value of <see cref="Idempotent"/>, or null when it was not set.</summary>
    public bool? GetIdempotent() => _idempotent;
}
