using System;

namespace Volo.Abp.MultiTenancy;

/// <summary>
/// Represents sides in a multi tenancy application.
/// </summary>
[Flags]
public enum MultiTenancySides
{
    /// <summary>
    /// Tenant side.
    /// </summary>
    Tenant = 1,

    /// <summary>
    /// Host side.
    /// </summary>
    Host = 2,

    /// <summary>
    /// Both sides
    /// </summary>
    Both = Tenant | Host
}
