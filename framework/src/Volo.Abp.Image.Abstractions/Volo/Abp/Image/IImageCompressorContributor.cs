using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.Image;

public interface IImageCompressorContributor
{
    Task<ImageContributorResult<Stream>> TryCompressAsync(Stream stream, string mimeType = null, CancellationToken cancellationToken = default);
    Task<ImageContributorResult<byte[]>> TryCompressAsync(byte[] bytes, string mimeType = null, CancellationToken cancellationToken = default);
}