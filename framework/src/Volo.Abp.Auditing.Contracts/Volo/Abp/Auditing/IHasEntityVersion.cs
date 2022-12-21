namespace Volo.Abp.Auditing;

/// <summary>
/// A version value that is increased whenever the entity is changed.
/// </summary>
public interface IHasEntityVersion
{
    /// <summary>
    /// A version value that is increased whenever the entity is changed.
    /// </summary>
    int EntityVersion { get; }
}