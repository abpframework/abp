using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.BlobStoring;

public abstract class BlobProviderBase : IBlobProvider
{
    public abstract Task SaveAsync(BlobProviderSaveArgs args);

    public abstract Task<bool> DeleteAsync(BlobProviderDeleteArgs args);

    public abstract Task<bool> ExistsAsync(BlobProviderExistsArgs args);

    public abstract Task<Stream> GetOrNullAsync(BlobProviderGetArgs args);

    protected virtual async Task<Stream> TryCopyToMemoryStreamAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        if (stream == null)
        {
            return null;
        }

        var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream, cancellationToken);
        memoryStream.Seek(0, SeekOrigin.Begin);
        return memoryStream;
    }
}
