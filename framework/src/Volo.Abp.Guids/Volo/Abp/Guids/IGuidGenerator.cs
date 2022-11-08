using System;

namespace Volo.Abp.Guids;

/// <summary>
/// Used to generate Ids.
/// </summary>
public interface IGuidGenerator
{
    /// <summary>
    /// Creates a new <see cref="Guid"/>.
    /// </summary>
    Guid Create();

    /// <summary>
    /// Generate a new <see cref="Guid"/> that conform to the RFC 4122 .
    /// </summary>
    /// <returns></returns>
    Guid Next();
}
