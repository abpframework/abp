namespace Volo.Abp.Auditing;

/// <summary>
/// An entity version property that auto-increments when the entity changes.
/// </summary>
public interface IHasEntityVersion
{
    /// <summary>
    /// An entity version property that auto-increments when the entity changes.
    /// </summary>
    int EntityVersion { get; }
}