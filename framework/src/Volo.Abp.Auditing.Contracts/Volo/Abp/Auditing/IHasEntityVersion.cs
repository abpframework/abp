namespace Volo.Abp.Auditing;

/// <summary>
/// A standard interface for automatic versioning of your entities.
/// </summary>
public interface IHasEntityVersion
{
    /// <summary>
    /// A version value that is increased whenever the entity is changed.
    /// </summary>
    int EntityVersion { get; }
}
