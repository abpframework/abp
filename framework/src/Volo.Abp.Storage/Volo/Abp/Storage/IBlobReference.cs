using System.IO;
using System.Threading.Tasks;

namespace Volo.Abp.Storage
{
    public interface IBlobReference : IPrivateBlobReference
    {
        string GetPublicBlobUrl { get; }

        IBlobDescriptor BlobDescriptor { get; }

        Task ReadBlobToStreamAsync(Stream targetStream);

        ValueTask<Stream> ReadBlobAsync();

        ValueTask<string> ReadBlobTextAsync();

        ValueTask<byte[]> ReadBlobBytesAsync();

        Task DeleteBlobAsync();

        Task UpdateBlobAsync(Stream stream);

        Task SaveBlobDescriptorAsync();

        ValueTask<string> GetBlobSasUrl(ISharedAccessPolicy policy);

        Task FetchBlobProperties();
    }
}