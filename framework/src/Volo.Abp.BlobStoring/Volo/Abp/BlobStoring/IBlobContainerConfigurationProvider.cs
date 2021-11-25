namespace Volo.Abp.BlobStoring;

public interface IBlobContainerConfigurationProvider
{
    /// <summary>
    /// Gets a <see cref="BlobContainerConfiguration"/> for the given container <paramref name="name"/>.
    /// </summary>
    /// <param name="name">The name of the container</param>
    /// <returns>The configuration that should be used for the container</returns>
    BlobContainerConfiguration Get(string name);
}
