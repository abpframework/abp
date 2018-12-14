using System.IO;
using System.Threading.Tasks;

namespace Volo.Abp.Storage
{
    public interface IFileReference : IPrivateFileReference
    {
        string PublicUrl { get; }

        IFileProperties Properties { get; }

        Task ReadToStreamAsync(Stream targetStream);

        ValueTask<Stream> ReadAsync();

        ValueTask<string> ReadAllTextAsync();

        ValueTask<byte[]> ReadAllBytesAsync();

        Task DeleteAsync();

        Task UpdateAsync(Stream stream);

        Task SavePropertiesAsync();

        ValueTask<string> GetSharedAccessSignature(ISharedAccessPolicy policy);

        Task FetchProperties();
    }
}
