using System.IO;
using System.Threading.Tasks;

namespace Volo.Abp.BlobStoring
{
    public abstract class BlobProviderBase : IBlobProvider
    {
        public abstract Task SaveAsync(BlobProviderSaveArgs args);

        public abstract Task<bool> DeleteAsync(BlobProviderDeleteArgs args);

        public abstract Task<bool> ExistsAsync(BlobProviderExistsArgs args);

        public virtual async Task<Stream> GetAsync(BlobProviderGetArgs args)
        {
            var result = await GetOrNullAsync(args);
            if (result == null)
            {
                //TODO: Consider to throw some type of "not found" exception and handle on the HTTP status side
                throw new AbpException($"Could not found the requested BLOB '{args.BlobName}' in the container '{args.ContainerName}'!");
            }

            return result;
        }

        public abstract Task<Stream> GetOrNullAsync(BlobProviderGetArgs args);
    }
}