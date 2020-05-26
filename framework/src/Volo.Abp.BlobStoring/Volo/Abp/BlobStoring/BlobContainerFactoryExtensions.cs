using System.Threading;

namespace Volo.Abp.BlobStoring
{
    public static class BlobContainerFactoryExtensions
    {
        /// <summary>
        /// Gets a named container.
        /// </summary>
        /// <param name="blobContainerFactory">The blob container manager</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>
        /// The container object.
        /// </returns>
        public static IBlobContainer Create<TContainer>(
            this IBlobContainerFactory blobContainerFactory,
            CancellationToken cancellationToken = default
        )
        {
            return blobContainerFactory.Create(
                BlobContainerNameAttribute.GetContainerName<TContainer>(),
                cancellationToken
            );
        }
    }
}