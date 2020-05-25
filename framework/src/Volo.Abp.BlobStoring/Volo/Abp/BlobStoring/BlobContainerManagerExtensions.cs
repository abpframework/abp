using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.BlobStoring
{
    public static class BlobContainerManagerExtensions
    {
        /// <summary>
        /// Gets a named container.
        /// </summary>
        /// <param name="blobContainerManager">The blob container manager</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>
        /// The container object.
        /// </returns>
        public static Task<IBlobContainer> GetAsync<TContainer>(
            this IBlobContainerManager blobContainerManager,
            CancellationToken cancellationToken = default
        )
        {
            return blobContainerManager.GetAsync(
                BlobContainerNameAttribute.GetContainerName<TContainer>(),
                cancellationToken
            );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blobContainerManager">The blob container manager</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <typeparam name="TContainer">Type of the container</typeparam>
        /// <returns>
        /// Returns true if actually deleted the container.
        /// Returns false if the container with the given <typeparamref name="TContainer"/> type was not exists.  
        /// </returns>
        public static Task<bool> DeleteAsync<TContainer>(
            this IBlobContainerManager blobContainerManager,
            CancellationToken cancellationToken = default
        )
        {
            return blobContainerManager.DeleteAsync(
                BlobContainerNameAttribute.GetContainerName<TContainer>(),
                cancellationToken
            );
        }
    }
}