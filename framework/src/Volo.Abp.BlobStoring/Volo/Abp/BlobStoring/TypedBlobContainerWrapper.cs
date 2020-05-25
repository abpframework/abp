using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.BlobStoring
{
    public class TypedBlobContainerWrapper<TContainer> : IBlobContainer<TContainer>
        where TContainer: class
    {
        private readonly IBlobContainer _container;

        public TypedBlobContainerWrapper(IBlobContainerFactory blobContainerFactory)
        {
            _container = blobContainerFactory.Create<TContainer>();
        }
        
        public Task SaveAsync(string name, Stream stream, bool overrideExisting = false, CancellationToken cancellationToken = default)
        {
            return _container.SaveAsync(name, stream, overrideExisting, cancellationToken);
        }

        public Task<bool> DeleteAsync(string name, CancellationToken cancellationToken = default)
        {
            return _container.DeleteAsync(name, cancellationToken);
        }

        public Task<bool> ExistsAsync(string name, CancellationToken cancellationToken = default)
        {
            return _container.ExistsAsync(name, cancellationToken);
        }

        public Task<Stream> GetAsync(string name, CancellationToken cancellationToken = default)
        {
            return _container.GetAsync(name, cancellationToken);
        }

        public Task<Stream> GetOrNullAsync(string name, CancellationToken cancellationToken = default)
        {
            return _container.GetOrNullAsync(name, cancellationToken);
        }
    }
}