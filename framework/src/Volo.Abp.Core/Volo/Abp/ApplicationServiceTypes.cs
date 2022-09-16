using System;

namespace Volo.Abp;

[Flags]
public enum ApplicationServiceTypes : byte
{
    /// <summary>
    /// Only application services without <see cref="IntegrationServiceAttribute"/>.
    /// </summary>
    ApplicationServices = 1,

    /// <summary>
    /// Application services with <see cref="IntegrationServiceAttribute"/>.
    /// </summary>
    IntegrationServices = 2,

    /// <summary>
    /// All application services.
    /// </summary>
    All = ApplicationServices | IntegrationServices
}