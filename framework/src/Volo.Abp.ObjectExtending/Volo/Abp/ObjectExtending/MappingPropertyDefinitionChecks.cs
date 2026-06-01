using System;

namespace Volo.Abp.ObjectExtending;

[Flags]
public enum MappingPropertyDefinitionChecks : byte
{
    /// <summary>
    /// Same as Null, We need to use this in Attribute to avoid null checks.
    /// </summary>
    Null = 0,

    /// <summary>
    /// No check. Copy all extra properties from the source to the destination.
    /// </summary>
    None = 1 << 0,

    /// <summary>
    /// Copy the extra properties defined for the source class.
    /// </summary>
    Source = 1 << 1,

    /// <summary>
    /// Copy the extra properties defined for the destination class.
    /// </summary>
    Destination = 1 << 2,

    /// <summary>
    /// Copy extra properties defined for both of the source and destination classes.
    /// </summary>
    Both = Source | Destination
}
