using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.BlobStoring;

public interface IBlobContainer<TContainer> : IBlobContainer
    where TContainer : class
{

}

public interface IBlobContainer
{
    /// <summary>
    /// Saves a blob <see cref="Stream"/> to the container.
    /// </summary>
    /// <param name="name">The name of the blob</param>
    /// <param name="stream">A stream for the blob</param>
    /// <param name="overrideExisting">
    /// Set <code>true</code> to override if there is already a blob in the container with the given name.
    /// If set to <code>false</code> (default), throws exception if there is already a blob in the container with the given name.
    /// </param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task SaveAsync(
        string name,
        Stream stream,
        bool overrideExisting = false,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Deletes a blob from the container.
    /// </summary>
    /// <param name="name">The name of the blob</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>
    /// Returns true if actually deleted the blob.
    /// Returns false if the blob with the given <paramref name="name"/> was not exists.  
    /// </returns>
    Task<bool> DeleteAsync(
        string name,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Checks if a blob does exists in the container.
    /// </summary>
    /// <param name="name">The name of the blob</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<bool> ExistsAsync(
        string name,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Gets a blob from the container.
    /// It actually gets a <see cref="Stream"/> to read the blob data.
    /// It throws exception if there is no blob with the given <paramref name="name"/>.
    /// Use <see cref="GetOrNullAsync"/> if you want to get <code>null</code> if there is no blob with the given <paramref name="name"/>. 
    /// </summary>
    /// <param name="name">The name of the blob</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>
    /// A <see cref="Stream"/> to read the blob data.
    /// </returns>
    Task<Stream> GetAsync(
        string name,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Gets a blob from the container, or returns null if there is no blob with the given <paramref name="name"/>.
    /// It actually gets a <see cref="Stream"/> to read the blob data.
    /// </summary>
    /// <param name="name">The name of the blob</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>
    /// A <see cref="Stream"/> to read the blob data.
    /// </returns>
    Task<Stream> GetOrNullAsync(
        string name,
        CancellationToken cancellationToken = default
    );

    //TODO: Create shortcut extension methods: GetAsArraryAsync, GetAsStringAsync(encoding) (and null versions)
}
