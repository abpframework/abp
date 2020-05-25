using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.BlobStoring
{
    public interface IBlobContainerManager
    {
        /// <summary>
        /// Gets a named container.
        /// </summary>
        /// <param name="name">The name of the container</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>
        /// The container object.
        /// </returns>
        Task<IBlobContainer> GetAsync(
            string name,
            CancellationToken cancellationToken = default
        );
        
        /// <summary>
        /// Deletes a container.
        /// </summary>
        /// <param name="name">The name of the container</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>
        /// Returns true if actually deleted the container.
        /// Returns false if the container with the given <paramref name="name"/> was not exists.  
        /// </returns>
        Task<bool> DeleteAsync(
            string name,
            CancellationToken cancellationToken = default
        );
    }
}