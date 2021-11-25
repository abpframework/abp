using System;

namespace Volo.Abp.ObjectExtending;

[Flags]
public enum MappingPropertyDefinitionChecks : byte
{
    /// <summary>
    /// No check. Copy all extra properties from the source to the destination.
    /// </summary>
    None = 0,

    /// <summary>
    /// Copy the extra properties defined for the source class.
    /// </summary>
    Source = 1,

    /// <summary>
    /// Copy the extra properties defined for the destination class.
    /// </summary>
    Destination = 2,

    /// <summary>
    /// Copy extra properties defined for both of the source and destination classes.
    /// </summary>
    Both = Source | Destination
}
